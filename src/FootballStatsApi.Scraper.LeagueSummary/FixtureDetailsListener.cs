using FootballStatsApi.Scraper.Shared.Messages;
using FootballStatsApi.Scraper.Shared;
using FootballStatsApi.Domain.Repositories;
using FootballStatsApi.Domain.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;

namespace FootballStatsApi.Scraper.LeagueSummary
{

    public class FixtureDetailsListener
    {
        private readonly ILogger<FixtureDetailsListener> _logger;
        private readonly IAmqpService _amqpService;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly FixtureDetailsScraper _scraper;
        private readonly IConnectionProvider _connectionProvider;
        private string _consumerTag;

        public FixtureDetailsListener(
            ILogger<FixtureDetailsListener> logger,
            IAmqpService amqpService,
            ICompetitionRepository competitionRepository,
            FixtureDetailsScraper scraper,
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
            _consumerTag = _amqpService.Consume(QueueName.GetFixtureDetails, ProcessMessage, autoAck: false);

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
                var message = JsonConvert.DeserializeObject<GetFixtureDetailsMessage>(body);
                await _scraper.Run(message.FixtureId);
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