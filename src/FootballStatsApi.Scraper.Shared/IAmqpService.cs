using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FootballStatsApi.Scraper.Shared
{
    public interface IAmqpService
    {
        void Send(IAmqpMessage message, string routingKey);
        Task Connect();
        void Declare();
        void Consume(string queueName, EventHandler<BasicDeliverEventArgs> consumer, bool autoAck);
        void Ack(ulong deliveryTag);
        void Nack(ulong deliveryTag, bool requeue);
    }
}