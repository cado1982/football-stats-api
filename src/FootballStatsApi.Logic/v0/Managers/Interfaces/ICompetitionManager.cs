using FootballStatsApi.Models.v0;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballStatsApi.Logic.v0.Managers
{
    public interface ICompetitionManager
    {
        Task<List<Competition>> GetAsync();
    }
}
