using System;
using System.Diagnostics;
using System.Threading.Tasks;
using RabbitMQ;

namespace FootballStatsApi.Scraper.Orchestration
{
    public class LeagueSummaryRequester
    {


        public LeagueSummaryRequester()
        {
            
        }
        public async Task Run()
        {
            Console.WriteLine("Timer fired");
            await Task.Delay(1000);
            await Run();
        }
    }
}