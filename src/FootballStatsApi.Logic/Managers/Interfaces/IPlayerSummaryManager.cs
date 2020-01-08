using FootballStatsApi.Models;
using System.Threading.Tasks;

namespace FootballStatsApi.Logic.Managers
{
    public interface IPlayerSummaryManager
    {
        Task<PlayerSummaries> GetAsync(int season, int competitionId);
        Task<PlayerSummary> GetByIdAsync(int playerId, int season, int competitionId);
    }
}
