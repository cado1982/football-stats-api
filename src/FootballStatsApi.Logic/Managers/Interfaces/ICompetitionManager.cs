using FootballStatsApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballStatsApi.Logic.Managers
{
    public interface ICompetitionManager
    {
        Task<List<Competition>> GetAsync();
    }
}
