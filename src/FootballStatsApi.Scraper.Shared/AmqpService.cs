using System;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using RabbitMQ.Client.Events;
using System.Text;

namespace FootballStatsApi.Scraper.Shared
{
    public class AmqpService : IAmqpService //, IDisposable
    {
        private readonly ILogger<AmqpService> _logger;
        private IConnection _connection;
        private IModel _channel;
        private ConnectionFactory _factory;
        private const string _exchangeName = "amq.topic";
        private int _connectAttempts = 0;

        public AmqpService(Uri amqpUri, ILogger<AmqpService> logger)
        {
            _logger = logger;

            _factory = new ConnectionFactory();
            _factory.AutomaticRecoveryEnabled = true;
            _factory.Uri = amqpUri;
        }

        public Task Send(IAmqpMessage message, string routingKey)
        {
            _channel.ExchangeDeclare(_exchangeName, ExchangeType.Topic, true);

            var json = JsonConvert.SerializeObject(message);
            byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(json);

            var props = _channel.CreateBasicProperties();
            _channel.BasicPublish(_exchangeName, routingKey, props, messageBodyBytes);

            return Task.CompletedTask;
        }

        public async Task Connect()
        {
            _connectAttempts++;
            var isFaulted = false;

            try
            {
                _connection = _factory.CreateConnection();
                _channel = _connection.CreateModel();

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
            _channel?.Close();
            _connection?.Close();

            _channel.Dispose();
            _connection.Dispose();
        }

        public Task DeclareQueue(string queueName, bool durable = true, bool exclusive = false, bool autoDelete = false, IDictionary<string, object> arguments = null)
        {
            _channel.QueueDeclare(queueName, durable, exclusive, autoDelete, arguments);
            return Task.CompletedTask;
        }



        public Task BindQueue(string queueName, string exchangeName, string routingKey)
        {
            _channel.QueueBind(queueName, exchangeName, routingKey);
            return Task.CompletedTask;
        }

        public async Task DeclareLeagueSummaryQueue()
        {
            var queueArgs = new Dictionary<string, object>
            {
                { "x-message-ttl", 60 * 60 * 1000 } // 1 hour
            };

            await DeclareQueue("getLeagueSummary", arguments: queueArgs);
            await BindQueue("getLeagueSummary", "amq.topic", "stats.getLeagueSummary");
        }

        public async Task DeclareFixtureDetailsQueue()
        {
            var queueArgs = new Dictionary<string, object>
            {
                { "x-message-ttl", 60 * 1000 } // 60 seconds
            };

            await DeclareQueue("getFixtureDetails", arguments: queueArgs);
            await BindQueue("getFixtureDetails", "amq.topic", "stats.getFixtureDetails");
        }

        public Task SubscribeToQueue<T>(string queueName, Action<T> action)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                var json = JsonConvert.DeserializeObject<T>(message);
                action(json);
            };
            _channel.BasicConsume(queue: queueName,
                                  autoAck: true,
                                  consumer: consumer);
            return Task.CompletedTask;
        }
    }
}