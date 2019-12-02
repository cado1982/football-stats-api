using System.Data;
using System.Threading.Tasks;
using FootballStatsApi.Scraper.LeagueSummary.Models;

namespace FootballStatsApi.Scraper.LeagueSummary
{
    public interface IFixtureDetailsManager
    {
        Task ProcessShots(FixtureShots shots, IDbConnection conn);
        Task ProcessRosters(FixtureRosters rosters, IDbConnection conn);
        Task ProcessMatchInfo(FixtureMatchInfo matchInfo, IDbConnection conn);
        Task ConfirmDetailsSaved(int fixtureId, IDbConnection conn);
    }
}