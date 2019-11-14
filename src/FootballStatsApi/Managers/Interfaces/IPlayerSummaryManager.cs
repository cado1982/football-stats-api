using FootballStatsApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballStatsApi.Managers
{
    public interface IPlayerSummaryManager
    {
        Task<PlayerSummaries> GetAsync(int season, int competitionId);
    }
}
