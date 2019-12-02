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
    public class FixtureDetailsRequester : IRequester
    {
        private int _delaySeconds = 60 * 5; // 5 minutes
        private bool _isRunning = false;
        private readonly ILogger<FixtureDetailsRequester> _logger;
        private readonly IAmqpService _amqpService;
        private readonly IFixtureRepository _fixtureRepository;
        private readonly IConnectionProvider _connectionProvider;
        private const string _routingKey = "stats.getFixtureDetails";

        public FixtureDetailsRequester(
            ILogger<FixtureDetailsRequester> logger,
            IAmqpService amqpService,
            IFixtureRepository fixtureRepository,
            IConnectionProvider connectionProvider)
        {
            _logger = logger;
            _amqpService = amqpService;
            _fixtureRepository = fixtureRepository;
            _connectionProvider = connectionProvider;
        }

        public async Task Run()
        {
            _logger.LogDebug("FixtureDetailsRequester Run()");

            await _amqpService.DeclareFixtureDetailsQueue();

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
                // 1. Get all fixtures that should've finished by now that we didn't save yet
                var fixtureIds = await _fixtureRepository.GetFixturesToCheckAsync(conn);

                // 2. Send an AMQP message to request the details be updated
                foreach (var fixtureId in fixtureIds)
                {
                    var message = new GetFixtureDetailsMessage();
                    message.FixtureId = fixtureId;

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
}