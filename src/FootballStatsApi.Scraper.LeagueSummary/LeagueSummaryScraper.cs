using System.Threading.Tasks;
using System;
using FootballStatsApi.Scraper.Shared;
using FootballStatsApi.Domain.Repositories;
using FootballStatsApi.Domain.Helpers;
using PuppeteerSharp;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace FootballStatsApi.Scraper.LeagueSummary
{
    public class LeagueSummaryScraper
    {
        private readonly IAmqpService _amqpService;
        private readonly ChromeHelper _chromeHelper;
        private readonly ILogger<LeagueSummaryScraper> _logger;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly ILeagueSummaryManager _leagueSummaryManager;
        private readonly IConnectionProvider _connectionProvider;

        public LeagueSummaryScraper(
            IAmqpService amqpService,
            ChromeHelper chromeHelper,
            ILogger<LeagueSummaryScraper> logger,
            ICompetitionRepository competitionRepository,
            ILeagueSummaryManager leagueSummaryManager,
            IConnectionProvider connectionProvider)
        {
            _competitionRepository = competitionRepository;
            _leagueSummaryManager = leagueSummaryManager;
            _connectionProvider = connectionProvider;
            _amqpService = amqpService;
            _chromeHelper = chromeHelper;
            _logger = logger;
        }

        public async Task Run(int competitionId, int? season = null)
        {
            try
            {
                _logger.LogDebug($"Entering Run for competition {competitionId}");

                using var conn = await _connectionProvider.GetOpenConnectionAsync();
                var browser = await Puppeteer.ConnectAsync(await _chromeHelper.GetConnectOptionsAsync());

                var competition = await _competitionRepository.GetByIdAsync(competitionId, conn);

                if (competition == null)
                {
                    throw new Exception($"Competition not found with id {competitionId}.");
                }

                using var page = await browser.NewPageAsync();
                
                var url = season != null ? $"https://understat.com/league/{competition.InternalName}/{season}" :
                                           $"https://understat.com/league/{competition.InternalName}";

                _logger.LogInformation("Loading " + url);

                var response = await page.GoToAsync(url, new NavigationOptions { WaitUntil = new WaitUntilNavigation[] { WaitUntilNavigation.Networkidle2 } });

                if (!response.Ok)
                {
                    _logger.LogError($"Unable to load {url}. Http Status {response.Status}");
                    return;
                }

                _logger.LogInformation("Attempting to parse season from page");
                // Use the select dropdown on the page to decide which season it is
                var seasonFromPage = await page.EvaluateExpressionAsync<int>("+document.querySelector('select[name=season] > option[selected]').value");
                _logger.LogInformation($"Season {seasonFromPage} found");

                var pd = await page.EvaluateExpressionAsync("playersData");
                var td = await page.EvaluateExpressionAsync("teamsData");
                var dd = await page.EvaluateExpressionAsync("datesData");

                var players = pd.ToObject<List<Models.Player>>();
                var teams = td.ToObject<Dictionary<string, Models.Team>>();
                var fixtures = dd.ToObject<List<Models.Fixture>>();

                await _leagueSummaryManager.ProcessTeams(teams.Values.ToList(), seasonFromPage, competition, conn);
                await _leagueSummaryManager.ProcessPlayers(players, seasonFromPage, competition, conn);
                await _leagueSummaryManager.ProcessFixtures(fixtures, seasonFromPage, competition, conn);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error running LeagueSummaryScraper");
                throw;
            }
        }
    }
}