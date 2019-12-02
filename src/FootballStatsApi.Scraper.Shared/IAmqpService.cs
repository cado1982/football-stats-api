using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballStatsApi.Scraper.Shared
{
    public interface IAmqpService
    {
        Task Send(IAmqpMessage message, string routingKey);
        Task Connect();
        Task DeclareQueue(string queueName, bool durable = true, bool exclusive = false, bool autoDelete = false, IDictionary<string, object> arguments = null);
        Task BindQueue(string queueName, string exchangeName, string routingKey);
        Task DeclareLeagueSummaryQueue();
        Task DeclareFixtureDetailsQueue();
        Task SubscribeToQueue<T>(string queueName, Action<T> action);
    }
}