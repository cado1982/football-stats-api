using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace FootballStatsApi.Scraper.Shared
{
    public interface IAmqpService
    {
        void Send(IAmqpMessage message, string routingKey);
        Task Connect();
        IModel Channel { get; }
    }
}