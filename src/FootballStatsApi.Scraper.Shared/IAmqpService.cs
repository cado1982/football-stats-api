using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FootballStatsApi.Scraper.Shared
{
    public interface IAmqpService : IDisposable
    {
        void Send(IAmqpMessage message, string routingKey);
        Task Connect(CancellationToken cancellationToken);
        Task Disconnect(CancellationToken cancellationToken);
        void Declare();
        string Consume(string queueName, EventHandler<BasicDeliverEventArgs> consumer, bool autoAck);
        void CancelConsume(string consumerTag);
        void Ack(ulong deliveryTag);
        void Nack(ulong deliveryTag, bool requeue);
    }
}