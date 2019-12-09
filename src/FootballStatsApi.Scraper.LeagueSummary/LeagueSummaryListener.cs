using FootballStatsApi.Scraper.Shared.Messages;
using FootballStatsApi.Scraper.Shared;
using FootballStatsApi.Domain.Repositories;
using FootballStatsApi.Domain.Helpers;
using System;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;

namespace FootballStatsApi.Scraper.LeagueSummary
{

    public class LeagueSummaryListener
    {
        private readonly ILogger<LeagueSummaryListener> _logger;
        private readonly IAmqpService _amqpService;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly LeagueSummaryScraper _scraper;
        private readonly IConnectionProvider _connectionProvider;
        private string _consumerTag;

        public LeagueSummaryListener(
            ILogger<LeagueSummaryListener> logger,
            IAmqpService amqpService,
            ICompetitionRepository competitionRepository,
            LeagueSummaryScraper scraper,
            IConnectionProvider connectionProvider)
        {
            _competitionRepository = competitionRepository;
            _scraper = scraper;
            _connectionProvider = connectionProvider;
            _logger = logger;
            _amqpService = amqpService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _consumerTag = _amqpService.Consume(QueueName.GetLeagueSummary, ProcessMessage, autoAck: false);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (String.IsNullOrWhiteSpace(_consumerTag)) return Task.CompletedTask;

            _amqpService.CancelConsume(_consumerTag);

            return Task.CompletedTask;
        }

        private async void ProcessMessage(object model, BasicDeliverEventArgs ea)
        {
            try
            {
                var body = Encoding.UTF8.GetString(ea.Body);
                var message = JsonConvert.DeserializeObject<GetLeagueSummaryMessage>(body);
                await _scraper.Run(message.CompetitionId, message.Season);
                _amqpService.Ack(ea.DeliveryTag);
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to process message");
            }

            try
            {
                _amqpService.Nack(ea.DeliveryTag, requeue: false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to nack amqp message");
                throw;
            }
        }
    }
}