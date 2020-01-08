using FootballStatsApi.Models;
using System.Threading.Tasks;

namespace FootballStatsApi.Logic.Managers
{
    public interface IFixtureManager
    {
        Task<FixtureBasic> GetDetailsAsync(int fixtureId);
        //Task<FixtureBasic> GetAsync(int? competitionId, int? season, int? teamId);
    }
}
