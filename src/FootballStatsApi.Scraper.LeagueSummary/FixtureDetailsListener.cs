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

namespace FootballStatsApi.Scraper.LeagueSummary
{

    public class FixtureDetailsListener : IListener
    {
        private readonly ILogger<FixtureDetailsListener> _logger;
        private readonly IAmqpService _amqpService;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly FixtureDetailsScraper _scraper;
        private readonly IConnectionProvider _connectionProvider;

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

        public void Listen()
        {
            _amqpService.Channel.QueueDeclare(QueueName.GetFixtureDetails, true, false, false, new Dictionary<string, object>
            {
                { "x-message-ttl", 60 * 60 * 1000 } // 1 hour
            });
            _amqpService.Channel.QueueBind(QueueName.GetFixtureDetails, ExchangeName.Topic, RoutingKey.FixtureDetails);

            var consumer = new EventingBasicConsumer(_amqpService.Channel);
            consumer.Received += async (model, ea) =>
            {
                var isFaulted = false;

                try
                {
                    var body = Encoding.UTF8.GetString(ea.Body);
                    var message = JsonConvert.DeserializeObject<GetFixtureDetailsMessage>(body);
                    await _scraper.Run(message.FixtureId);
                    _amqpService.Channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unable to process message");
                    isFaulted = true;
                }

                if (isFaulted) 
                {
                    try
                    {
                        _amqpService.Channel.BasicNack(ea.DeliveryTag, false, false);    
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Unable to nack amqp message");
                        throw;
                    }
                }
            };

            _amqpService.Channel.BasicConsume(QueueName.GetFixtureDetails, false, consumer);
        }
    }
}