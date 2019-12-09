using System;
using System.Threading;
using System.Threading.Tasks;
using FootballStatsApi.Scraper.Orchestration.Requesters;
using FootballStatsApi.Scraper.Shared;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FootballStatsApi.Scraper.Orchestration
{
    public class AppService : IHostedService, IDisposable
    {
        private readonly ILogger<AppService> _logger;
        private readonly IAmqpService _amqpService;
        private readonly FixtureDetailsRequester _fixtureDetailsRequester;
        private readonly LeagueSummaryRequester _leagueSummaryRequester;

        public AppService(
            ILogger<AppService> logger,
            IAmqpService amqpService, 
            FixtureDetailsRequester fixtureDetailsRequester,
            LeagueSummaryRequester leagueSummaryRequester)
        {
            _logger = logger;
            _amqpService = amqpService;
            _fixtureDetailsRequester = fixtureDetailsRequester;
            _leagueSummaryRequester = leagueSummaryRequester;
        }
        public void Dispose()
        {
            _amqpService.Dispose();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _amqpService.Connect(cancellationToken);
            _amqpService.Declare();

            await _fixtureDetailsRequester.StartAsync(cancellationToken);
            await _leagueSummaryRequester.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _fixtureDetailsRequester.StopAsync(cancellationToken);
            await _leagueSummaryRequester.StopAsync(cancellationToken);
            await _amqpService.Disconnect(cancellationToken);
        }
    }
}
