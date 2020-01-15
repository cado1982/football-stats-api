using FootballStatsApi.Models.v0;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballStatsApi.Logic.v0.Managers
{
    public interface IFixtureManager
    {
        //Task<FixtureBasic> GetFixtureBasicAsync(int fixtureId);
        Task<List<FixtureBasic>> GetFixturesBasicAsync(int competitionId, int season);
    }
}
