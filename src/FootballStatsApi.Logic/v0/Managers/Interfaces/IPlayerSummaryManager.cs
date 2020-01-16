using FootballStatsApi.Models.v0;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballStatsApi.Logic.v0.Managers
{
    public interface IPlayerSummaryManager
    {
        Task<List<PlayerSummaryBasic>> GetAsync(int season, int competitionId);
        Task<PlayerSummaryBasic> GetByIdAsync(int playerId, int season);
    }
}
