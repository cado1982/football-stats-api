using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FootballStatsApi.Scraper.Orchestration.Requesters
{
    public abstract class TimedService : IHostedService
    {
        protected abstract TimeSpan TimerInterval { get; } 
        private readonly ILogger<TimedService> _logger;
        private Timer _timer;

        public TimedService(ILogger<TimedService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting");

            _timer = new Timer(Process, null, TimeSpan.Zero, TimerInterval);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        protected abstract void Process(object state);
    }
}