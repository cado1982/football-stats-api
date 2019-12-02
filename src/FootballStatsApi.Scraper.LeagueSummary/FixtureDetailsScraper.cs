using RabbitMQ.Client;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using System.Text;
using FootballStatsApi.Scraper.Shared.Messages;
using FootballStatsApi.Scraper.Shared;
using RabbitMQ.Client.Events;
using FootballStatsApi.Domain.Repositories;
using FootballStatsApi.Domain.Helpers;
using PuppeteerSharp;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Entities = FootballStatsApi.Domain.Entities;
using Models = FootballStatsApi.Scraper.LeagueSummary.Models;
using System.Data;
using FootballStatsApi.Domain.Entities;
using System.Linq;
using FootballStatsApi.Scraper.LeagueSummary.Models;

namespace FootballStatsApi.Scraper.LeagueSummary
{
    public class FixtureDetailsScraper
    {
        private readonly IAmqpService _amqpService;
        private readonly ILogger<FixtureDetailsScraper> _logger;
        private readonly Browser _browser;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly IFixtureDetailsManager _fixtureDetailsManager;
        private readonly IConnectionProvider _connectionProvider;

        public FixtureDetailsScraper(
            Browser browser,
            IAmqpService amqpService,
            ILogger<FixtureDetailsScraper> logger,
            ICompetitionRepository competitionRepository,
            IFixtureDetailsManager fixtureDetailsManager,
            IConnectionProvider connectionProvider)
        {
            _competitionRepository = competitionRepository;
            _fixtureDetailsManager = fixtureDetailsManager;
            _connectionProvider = connectionProvider;
            _amqpService = amqpService;
            _logger = logger;
            _browser = browser;
        }

        public async Task Run(int fixtureId)
        {
            try
            {
                _logger.LogDebug($"Entering Run({fixtureId})");

                using (var conn = _connectionProvider.GetOpenConnection())
                using (var trans = conn.BeginTransaction())
                {
                    var page = await _browser.NewPageAsync();

                    var url = $"https://understat.com/match/{fixtureId}";

                    _logger.LogInformation("Loading " + url);

                    var response = await page.GoToAsync(url, new NavigationOptions { WaitUntil = new WaitUntilNavigation[] { WaitUntilNavigation.Networkidle2 } });

                    if (!response.Ok)
                    {
                        _logger.LogError($"Unable to load {url}. Http Status {response.Status}");
                        return;
                    }

                    var sd = await page.EvaluateExpressionAsync("shotsData");
                    var rd = await page.EvaluateExpressionAsync("rostersData");
                    var mi = await page.EvaluateExpressionAsync("match_info");

                    var shots = sd.ToObject<FixtureShots>();
                    var rosters = rd.ToObject<FixtureRosters>();
                    var matchInfo = mi.ToObject<FixtureMatchInfo>();

                    await _fixtureDetailsManager.ProcessShots(shots, conn);
                    await _fixtureDetailsManager.ProcessRosters(rosters, conn);
                    await _fixtureDetailsManager.ProcessMatchInfo(matchInfo, conn);
                    await _fixtureDetailsManager.ConfirmDetailsSaved(fixtureId, conn);

                    trans.Commit();

                    await page.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error running FixtureDetailsScraper for fixture {fixtureId}");
            }
        }
    }
}