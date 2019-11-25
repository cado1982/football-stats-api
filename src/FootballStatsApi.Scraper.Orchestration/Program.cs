using System;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace FootballStatsApi.Scraper.Orchestration
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@localhost/football");
            var conn = factory.CreateConnection();
            var channel = conn.CreateModel();

            channel.ExchangeDeclare("amq.topic", ExchangeType.Topic, true);
            var props = channel.CreateBasicProperties();
            byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes("Hello, world!");

            while (true)
            {
                channel.BasicPublish("amq.topic", "stats.leagueSummary", props, messageBodyBytes);
                //Task.Delay(1).Wait();
            }

            channel.Close();
            conn.Close();
            // var req = new LeagueSummaryRequester();
            // req.Run().Wait();

            Console.WriteLine("Done");
        }
    }
}
