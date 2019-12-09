using System;
using FootballStatsApi.Domain.Helpers;
using FootballStatsApi.Domain.Repositories;
using FootballStatsApi.Scraper.Shared;
using FootballStatsApi.Scraper.Shared.Messages;
using Microsoft.Extensions.Logging;

namespace FootballStatsApi.Scraper.Orchestration.Requesters
{
    public class FixtureDetailsRequester : TimedService
    {
        private readonly ILogger<FixtureDetailsRequester> _logger;
        private readonly IAmqpService _amqpService;
        private readonly IFixtureRepository _fixtureRepository;
        private readonly IConnectionProvider _connectionProvider;

        protected override TimeSpan TimerInterval => TimeSpan.FromSeconds(Timers.FixtureDetailsIntervalSeconds);

        public FixtureDetailsRequester(
            ILogger<FixtureDetailsRequester> logger,
            IAmqpService amqpService,
            IFixtureRepository fixtureRepository,
            IConnectionProvider connectionProvider) : base(logger)
        {
            _logger = logger;
            _amqpService = amqpService;
            _fixtureRepository = fixtureRepository;
            _connectionProvider = connectionProvider;
        }

        protected override async void Process(object state)
        {
            _logger.LogInformation("Running process iteration");

            using (var conn = await _connectionProvider.GetOpenConnectionAsync())
            {
                // 1. Get all fixtures that should've finished by now that we didn't save yet
                var fixtureIds = await _fixtureRepository.GetFixturesToCheckAsync(conn);

                _logger.LogInformation($"Found {fixtureIds.Count} to process");

                // 2. Send an AMQP message to request the details be updated
                foreach (var fixtureId in fixtureIds)
                {
                    var message = new GetFixtureDetailsMessage();
                    message.FixtureId = fixtureId;

                    _amqpService.Send(message, RoutingKey.FixtureDetails);
                }

                _logger.LogInformation($"Sent {fixtureIds.Count} AMQP messages to request fixture details");
            }
        }
    }
}