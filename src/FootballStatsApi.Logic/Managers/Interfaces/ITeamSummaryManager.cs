using FootballStatsApi.Models;
using System.Threading.Tasks;

namespace FootballStatsApi.Logic.Managers
{
    public interface ITeamSummaryManager
    {
        Task<TeamSummaries> GetAsync(int season, int competitionId);
        Task<TeamSummary> GetByIdAsync(int teamId, int season, int competitionId);
    }
}
