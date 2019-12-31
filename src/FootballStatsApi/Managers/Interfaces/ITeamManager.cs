using FootballStatsApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballStatsApi.Managers
{
    public interface ITeamManager
    {
        Task<List<Team>> GetAsync();
        Task<List<Team>> GetAsync(int season, int competitionId);
    }
}