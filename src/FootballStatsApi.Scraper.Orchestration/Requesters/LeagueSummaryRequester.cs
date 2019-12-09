using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FootballStatsApi.Domain.Helpers;
using FootballStatsApi.Domain.Repositories;
using FootballStatsApi.Scraper.Shared;
using FootballStatsApi.Scraper.Shared.Messages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ;

namespace FootballStatsApi.Scraper.Orchestration.Requesters
{

    public class LeagueSummaryRequester : TimedService
    {
        private readonly ILogger<LeagueSummaryRequester> _logger;
        private readonly IAmqpService _amqpService;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly IConnectionProvider _connectionProvider;

        public LeagueSummaryRequester(
            ILogger<LeagueSummaryRequester> logger,
            IAmqpService amqpService,
            ICompetitionRepository competitionRepository,
            IConnectionProvider connectionProvider) : base(logger)
        {
            _logger = logger;
            _amqpService = amqpService;
            _competitionRepository = competitionRepository;
            _connectionProvider = connectionProvider;
        }

        protected override TimeSpan TimerInterval { get => TimeSpan.FromSeconds(Timers.LeagueSummaryIntervalSeconds); }

        protected override async void Process(object state)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to process LeagueSummary iteration");
                throw;
            }
        }
    }
}