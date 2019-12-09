using System;
using System.Threading;
using System.Threading.Tasks;
using FootballStatsApi.Scraper.Shared;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FootballStatsApi.Scraper.LeagueSummary
{
    public class AppService : IHostedService, IDisposable
    {
        private readonly ILogger<AppService> _logger;
        private readonly IAmqpService _amqpService;
        private readonly LeagueSummaryListener _leagueSummaryListener;
        private readonly FixtureDetailsListener _fixtureDetailsListener;

        public AppService(
            ILogger<AppService> logger,
            IAmqpService amqpService, 
            LeagueSummaryListener leagueSummaryListener,
            FixtureDetailsListener fixtureDetailsListener)
        {
            _fixtureDetailsListener = fixtureDetailsListener;
            _leagueSummaryListener = leagueSummaryListener;
            _logger = logger;
            _amqpService = amqpService;
        }
        public void Dispose()
        {
            _amqpService.Dispose();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _amqpService.Connect(cancellationToken);
            _amqpService.Declare();
            await _leagueSummaryListener.StartAsync(cancellationToken);
            await _fixtureDetailsListener.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _leagueSummaryListener.StopAsync(cancellationToken);
            await _fixtureDetailsListener.StopAsync(cancellationToken);
            await _amqpService.Disconnect(cancellationToken);
        }
    }
}
