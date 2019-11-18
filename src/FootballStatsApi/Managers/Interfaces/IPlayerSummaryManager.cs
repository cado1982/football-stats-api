using FootballStatsApi.Models;
using System.Threading.Tasks;

namespace FootballStatsApi.Managers
{
    public interface IPlayerSummaryManager
    {
        Task<PlayerSummaries> GetAsync(int season, int competitionId);
        Task<PlayerSummary> GetByIdAsync(int playerId, int season, int competitionId);
    }
}
