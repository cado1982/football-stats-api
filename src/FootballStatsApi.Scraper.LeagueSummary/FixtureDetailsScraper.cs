using System.Threading.Tasks;
using System;
using FootballStatsApi.Scraper.Shared;
using FootballStatsApi.Domain.Repositories;
using FootballStatsApi.Domain.Helpers;
using PuppeteerSharp;
using Microsoft.Extensions.Logging;
using FootballStatsApi.Scraper.LeagueSummary.Models;

namespace FootballStatsApi.Scraper.LeagueSummary
{
    public class FixtureDetailsScraper
    {
        private readonly ILogger<FixtureDetailsScraper> _logger;
        private readonly ChromeHelper _chromeHelper;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly IFixtureDetailsManager _fixtureDetailsManager;
        private readonly IConnectionProvider _connectionProvider;

        public FixtureDetailsScraper(
            ILogger<FixtureDetailsScraper> logger,
            ChromeHelper chromeHelper,
            ICompetitionRepository competitionRepository,
            IFixtureDetailsManager fixtureDetailsManager,
            IConnectionProvider connectionProvider)
        {
            _competitionRepository = competitionRepository;
            _fixtureDetailsManager = fixtureDetailsManager;
            _connectionProvider = connectionProvider;
            _logger = logger;
            _chromeHelper = chromeHelper;
        }

        public async Task Run(int fixtureId)
        {
            try
            {
                _logger.LogDebug($"Entering Run({fixtureId})");

                FixtureRosters rosters = null;
                FixtureShots shots = null;
                FixtureMatchInfo matchInfo = null;

                using (var conn = await _connectionProvider.GetOpenConnectionAsync())
                using (var trans = conn.BeginTransaction())
                {
                    var browser = await Puppeteer.ConnectAsync(await _chromeHelper.GetConnectOptionsAsync());

                    try
                    {
                        using (var page = await browser.NewPageAsync())
                        {
                            var url = $"https://understat.com/match/{fixtureId}";

                            _logger.LogInformation("Loading " + url);

                            var response = await page.GoToAsync(url, new NavigationOptions { WaitUntil = new WaitUntilNavigation[] { WaitUntilNavigation.Networkidle2 } });

                            if (!response.Ok)
                            {
                                _logger.LogError($"Unable to load {url}. Http Status {response.Status}");
                                return;
                            }

                            _logger.LogInformation($"Success loading {url}");

                            var sd = await page.EvaluateExpressionAsync("shotsData");
                            var rd = await page.EvaluateExpressionAsync("rostersData");
                            var mi = await page.EvaluateExpressionAsync("match_info");

                            shots = sd.ToObject<FixtureShots>();
                            rosters = rd.ToObject<FixtureRosters>();
                            matchInfo = mi.ToObject<FixtureMatchInfo>();   
                        }
                    }
                    finally
                    {
                        browser.Disconnect();
                    }

                    await _fixtureDetailsManager.ProcessRosters(rosters, shots, matchInfo, conn);
                    await _fixtureDetailsManager.ProcessShots(shots, rosters, matchInfo, conn);
                    await _fixtureDetailsManager.ProcessMatchInfo(matchInfo, conn);
                    await _fixtureDetailsManager.ConfirmDetailsSaved(fixtureId, conn);

                    trans.Commit();

                    _logger.LogDebug($"Exiting Run({fixtureId})");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error running FixtureDetailsScraper for fixture {fixtureId}");
                throw;
            }
        }
    }
}