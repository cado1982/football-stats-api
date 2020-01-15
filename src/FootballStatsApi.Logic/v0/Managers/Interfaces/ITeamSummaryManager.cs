using FootballStatsApi.Models.v0;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballStatsApi.Logic.v0.Managers
{
    public interface ITeamSummaryManager
    {
        Task<List<TeamSummaryBasic>> GetAsync(int season, int competitionId);
        Task<TeamSummaryBasic> GetByIdAsync(int teamId, int season);
        Task<List<TeamSummaryBasic>> GetByIdAsync(int teamId);
    }
}
