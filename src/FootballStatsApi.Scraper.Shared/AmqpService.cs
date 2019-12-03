using System;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FootballStatsApi.Scraper.Shared
{
    public class AmqpService : IAmqpService, IDisposable
    {
        private readonly ILogger<AmqpService> _logger;
        private IConnection _connection;
        private ConnectionFactory _factory;
        private int _connectAttempts = 0;

        public IModel Channel { get; private set; }

        public AmqpService(Uri amqpUri, ILogger<AmqpService> logger)
        {
            _logger = logger;

            _factory = new ConnectionFactory();
            _factory.AutomaticRecoveryEnabled = true;
            _factory.Uri = amqpUri;
        }

        public void Send(IAmqpMessage message, string routingKey)
        {
            var json = JsonConvert.SerializeObject(message);
            byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(json);

            var props = Channel.CreateBasicProperties();
            Channel.BasicPublish(ExchangeName.Topic, routingKey, props, messageBodyBytes);
        }

        public async Task Connect()
        {
            _logger.LogInformation("Attempting to connect to RabbitMQ");
            _connectAttempts++;
            var isFaulted = false;

            try
            {
                _connection = _factory.CreateConnection();
                Channel = _connection.CreateModel();
                Channel.BasicQos(0, 1, false); // Only pull one message at a time

                _connectAttempts = 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to connect to RabbitMQ. Attempt {_connectAttempts}.");

                isFaulted = true;
            }

            // The reconnect must not be done inside the catch block
            // It's bad practice to have code in a catch block that could throw further exceptions
            if (isFaulted)
            {
                await Task.Delay(5000);
                await Connect();
            }
        }

        public void Dispose()
        {
            Channel?.Close();
            _connection?.Close();

            Channel.Dispose();
            _connection.Dispose();
        }
    }
}