using FootballStatsApi.Scraper.Shared.Messages;
using FootballStatsApi.Scraper.Shared;
using FootballStatsApi.Domain.Repositories;
using FootballStatsApi.Domain.Helpers;
using System;
using RabbitMQ.Client.Events;
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
            _amqpService.Consume(QueueName.GetLeagueSummary, ProcessMessage, autoAck: false);
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