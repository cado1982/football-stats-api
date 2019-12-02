using System.Threading.Tasks;

namespace FootballStatsApi.Scraper.Orchestration.Requesters
{
    public interface IRequester
    {
        Task Run();
        void Stop();
    }
}