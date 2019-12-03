using System.Threading.Tasks;
using FootballStatsApi.Scraper.Shared.Messages;
using FootballStatsApi.Scraper.Shared;
using FootballStatsApi.Domain.Repositories;
using FootballStatsApi.Domain.Helpers;
using System.Collections.Generic;
using System;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace FootballStatsApi.Scraper.LeagueSummary
{

    public class LeagueSummaryListener : IListener
    {
        private readonly ILogger<LeagueSummaryListener> _logger;
        private readonly IAmqpService _amqpService;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly LeagueSummaryScraper _scraper;
        private readonly IConnectionProvider _connectionProvider;

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

        public void Listen()
        {
            _amqpService.Channel.QueueDeclare(QueueName.GetLeagueSummary, true, false, false, new Dictionary<string, object>
            {
                { "x-message-ttl", 60 * 60 * 1000 } // 1 hour
            });
            _amqpService.Channel.QueueBind(QueueName.GetLeagueSummary, ExchangeName.Topic, RoutingKey.LeagueSummary);

            var consumer = new EventingBasicConsumer(_amqpService.Channel);
            consumer.Received += async (model, ea) =>
            {
                var isFaulted = false;

                try
                {
                    var body = Encoding.UTF8.GetString(ea.Body);
                    var message = JsonConvert.DeserializeObject<GetLeagueSummaryMessage>(body);
                    await _scraper.Run(message.CompetitionId, message.Season);
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
            _amqpService.Channel.BasicConsume(QueueName.GetLeagueSummary, false, consumer);
        }
    }
}