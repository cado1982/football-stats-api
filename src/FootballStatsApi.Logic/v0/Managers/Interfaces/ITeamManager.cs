using FootballStatsApi.Models.v0;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballStatsApi.Logic.v0.Managers
{
    public interface ITeamManager
    {
        Task<List<Team>> GetAsync();
        Task<List<Team>> GetAsync(int season, int competitionId);
        //Task<List<TeamBasicStats>> GetBasicStatsAsync(int season, int competitionId);
    }
}