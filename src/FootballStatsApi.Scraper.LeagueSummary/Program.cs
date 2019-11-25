using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FootballStatsApi.Scraper.LeagueSummary
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { VirtualHost = "football" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                channel.QueueBind("hello", "amq.topic", "stats.leagueSummary");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);
                    channel.BasicAck(ea.DeliveryTag, false);
                };
                channel.BasicConsume(queue: "hello",
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
