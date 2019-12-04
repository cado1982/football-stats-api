using System;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using RabbitMQ.Client.Events;

namespace FootballStatsApi.Scraper.Shared
{
    public class AmqpService : IAmqpService, IDisposable
    {
        private readonly ILogger<AmqpService> _logger;
        private IConnection _connection;
        private ConnectionFactory _factory;
        private int _connectAttempts = 0;
        private IModel _channel;

        public AmqpService(Uri amqpUri, ILogger<AmqpService> logger)
        {
            _logger = logger;

            _factory = new ConnectionFactory();
            _factory.AutomaticRecoveryEnabled = true;
            _factory.Uri = amqpUri;
        }

        public async Task Connect()
        {
            _logger.LogInformation("Attempting to connect to RabbitMQ");
            _connectAttempts++;

            try
            {
                _connection = _factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.BasicQos(0, 1, false); // Only pull one message at a time

                _connectAttempts = 0;
                _logger.LogInformation("Successfully connected to RabbitMQ");
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to connect to RabbitMQ. Attempt {_connectAttempts}.");
            }

            // This code will only run if the catch block was hit above.
            await Task.Delay(5000);
            await Connect();
        }

        public void Send(IAmqpMessage message, string routingKey)
        {
            if (_channel == null) throw new InvalidOperationException("Channel is not been established");
            
            var json = JsonConvert.SerializeObject(message);
            byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(json);

            var props = _channel.CreateBasicProperties();
            _channel.BasicPublish(ExchangeName.Topic, routingKey, props, messageBodyBytes);
        }

        public void Consume(string queueName, EventHandler<BasicDeliverEventArgs> messageHandler, bool autoAck)
        {
            if (_channel == null) throw new InvalidOperationException("Channel is not been established");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += messageHandler;

            _channel.BasicConsume(queueName, autoAck, consumer);
        }

        public void Declare()
        {
            if (_channel == null) throw new InvalidOperationException("Channel is not been established");

            DeclareExchanges();
            DeclareQueues();
        }

        public void Ack(ulong deliveryTag)
        {
            if (_channel == null) throw new InvalidOperationException("Channel is not been established");

            _channel.BasicAck(deliveryTag, false);
        }

        public void Nack(ulong deliveryTag, bool requeue)
        {
            if (_channel == null) throw new InvalidOperationException("Channel is not been established");

            _channel.BasicNack(deliveryTag, false, requeue);
        }

        private void DeclareExchanges()
        {
            _channel.ExchangeDeclare(ExchangeName.Topic, "topic", true, false);
            _channel.ExchangeDeclare(ExchangeName.TopicRetry, "topic", true, false);
        }

        private void DeclareQueues()
        {
            DeclareFixtureDetailsQueue();
            DeclareLeagueSummaryQueue();
        }

        private void DeclareFixtureDetailsQueue()
        {
            _channel.QueueDeclare(QueueName.GetFixtureDetailsRetry, true, false, false, new Dictionary<string, object>
            {
                { "x-message-ttl", Timers.FixtureDetailsRetryBackoffSeconds * 1000 },
                { "x-dead-letter-exchange", ExchangeName.Topic }
            });
            _channel.QueueBind(QueueName.GetFixtureDetailsRetry, ExchangeName.TopicRetry, RoutingKey.FixtureDetails);

            _channel.QueueDeclare(QueueName.GetFixtureDetails, true, false, false, new Dictionary<string, object>
            {
                { "x-message-ttl", Timers.FixtureDetailsIntervalSeconds * 1000 }, // This should match the request interval so no duplicate messages are possible
                { "x-dead-letter-exchange", ExchangeName.TopicRetry }
            });
            _channel.QueueBind(QueueName.GetFixtureDetails, ExchangeName.Topic, RoutingKey.FixtureDetails);
        }

        private void DeclareLeagueSummaryQueue()
        {
            _channel.QueueDeclare(QueueName.GetLeagueSummaryRetry, true, false, false, new Dictionary<string, object>
            {
                { "x-message-ttl", Timers.LeagueSummaryRetryBackoffSeconds * 1000 },
                { "x-dead-letter-exchange", ExchangeName.Topic }
            });
            _channel.QueueBind(QueueName.GetLeagueSummaryRetry, ExchangeName.TopicRetry, RoutingKey.LeagueSummary);

            _channel.QueueDeclare(QueueName.GetLeagueSummary, true, false, false, new Dictionary<string, object>
            {
                { "x-message-ttl", Timers.LeagueSummaryIntervalSeconds * 1000 }, // This should match the request interval so no duplicate messages are possible
                { "x-dead-letter-exchange", ExchangeName.TopicRetry }
            });
            _channel.QueueBind(QueueName.GetLeagueSummary, ExchangeName.Topic, RoutingKey.LeagueSummary);
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();

            _channel.Dispose();
            _connection.Dispose();
        }
    }
}