using RabbitMQ.Client;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using System.Text;

namespace FootballStatsApi.Scraper.LeagueSummary
{
    public class LeagueSummaryListener : DefaultBasicConsumer
    {
        // public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body)
        // {
        //     var message = Encoding.UTF8.GetString(body);
        //     var json = JsonConvert.DeserializeObject<GetLeagueSummaryMessage>(message);


        // }
//         public Task Listen()
//         {
//             var factory = new ConnectionFactory();
//             factory.Uri = new Uri("amqp://guest:guest@localhost/football");
//             var conn = factory.CreateConnection();
//             var channel = conn.CreateModel();

// channel.ExchangeDeclare("amq.topic", ExchangeType.Topic);

//             channel.Close(); 
//             conn.Close();

            
//         }
    }
}