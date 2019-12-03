using System.Data;
using System.Threading.Tasks;
using FootballStatsApi.Scraper.LeagueSummary.Models;

namespace FootballStatsApi.Scraper.LeagueSummary
{
    public interface IFixtureDetailsManager
    {
        Task ProcessShots(FixtureShots shots, FixtureRosters rosters, FixtureMatchInfo matchInfo, IDbConnection conn);
        Task ProcessRosters(FixtureRosters rosters, FixtureMatchInfo fixtureMatchInfo, IDbConnection conn);
        Task ProcessMatchInfo(FixtureMatchInfo matchInfo, IDbConnection conn);
        Task ConfirmDetailsSaved(int fixtureId, IDbConnection conn);
    }
}