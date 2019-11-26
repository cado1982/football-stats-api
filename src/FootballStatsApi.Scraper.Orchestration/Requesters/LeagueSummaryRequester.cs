using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private int _delaySeconds = 60;
        private bool _isRunning = false;
        private readonly ILogger<LeagueSummaryRequester> _logger;
        private readonly IAmqpService _amqpService;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly IConnectionProvider _connectionProvider;
        private const string _routingKey = "stats.getLeagueSummary";

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

            await _amqpService.DeclareLeagueSummaryQueue();
            await _amqpService.BindQueue("getLeagueSummary", "amq.topic", "stats.getLeagueSummary");

            _isRunning = true;

            await Process();

            while (_isRunning)
            {
                await Task.Delay(_delaySeconds * 1000);
                await Process();
            }
        }

        private async Task Process()
        {
            _logger.LogInformation("Running process iteration");

            using (var conn = _connectionProvider.GetOpenConnection())
            {
                // 1. Get all competitions
                var competitions = await _competitionRepository.GetAsync(conn);

                // 2. Fire off a GetLeagueSummaryMessage for each competition
                foreach (var competition in competitions)
                {
                    var message = new GetLeagueSummaryMessage();
                    message.Competition = competition.Id;

                    _logger.LogInformation($"Sending AMQP message to '{_routingKey}'. {JsonConvert.SerializeObject(message)}");
                    await _amqpService.Send(message, _routingKey);
                }
            }
        }

        public void Stop()
        {
            this._isRunning = false;
        }
    }

    public interface IRequester
    {
        Task Run();
        void Stop();
    }
}