using FootballStatsApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballStatsApi.Logic.Managers
{
    public interface IFixtureManager
    {
        //Task<FixtureBasic> GetFixtureBasicAsync(int fixtureId);
        Task<List<FixtureBasic>> GetFixturesBasicAsync(int competitionId, int season);
    }
}
