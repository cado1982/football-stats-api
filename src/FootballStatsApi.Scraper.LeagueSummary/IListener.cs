using System.Threading.Tasks;

namespace FootballStatsApi.Scraper.LeagueSummary
{
    public interface IListener
    {
        Task Listen();
    }
}