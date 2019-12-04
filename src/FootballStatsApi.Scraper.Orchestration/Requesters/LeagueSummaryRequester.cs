using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FootballStatsApi.Domain.Helpers;
using FootballStatsApi.Domain.Repositories;
using FootballStatsApi.Scraper.Shared;
using FootballStatsApi.Scraper.Shared.Messages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ;

namespace FootballStatsApi.Scraper.Orchestration.Requesters
{
    public class LeagueSummaryRequester : IRequester
    {
        private bool _isRunning = false;
        private readonly ILogger<LeagueSummaryRequester> _logger;
        private readonly IAmqpService _amqpService;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly IConnectionProvider _connectionProvider;

        public LeagueSummaryRequester(
            ILogger<LeagueSummaryRequester> logger,
            IAmqpService amqpService,
            ICompetitionRepository competitionRepository,
            IConnectionProvider connectionProvider)
        {
            _logger = logger;
            _amqpService = amqpService;
            _competitionRepository = competitionRepository;
            _connectionProvider = connectionProvider;
        }

        public async Task Run()
        {
            _logger.LogDebug("LeagueSummaryRequester Run()");

            _isRunning = true;

            await Process();

            while (_isRunning)
            {
                await Task.Delay(Timers.LeagueSummaryIntervalSeconds * 1000);
                await Process();
            }
        }

        private async Task Process()
        {
            _logger.LogInformation("Running process iteration");

            using (var conn = await _connectionProvider.GetOpenConnectionAsync())
            {
                // 1. Get all competitions
                var competitions = await _competitionRepository.GetAsync(conn);

                // 2. Fire off a GetLeagueSummaryMessage for each competition
                foreach (var competition in competitions)
                {
                    var message = new GetLeagueSummaryMessage();
                    message.CompetitionId = competition.Id;

                    _logger.LogInformation($"Sending AMQP message to '{RoutingKey.LeagueSummary}'. {JsonConvert.SerializeObject(message)}");
                    _amqpService.Send(message, RoutingKey.LeagueSummary);
                }
            }
        }

        public void Stop()
        {
            this._isRunning = false;
        }
    }
}