using FootballStatsApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace FootballStatsApi.Domain.Repositories
{
    public interface ITeamRepository
    {
        Task<List<Team>> GetAllTeamsAsync(IDbConnection connection);
        Task<List<Team>> GetTeamsAsync(int competitionId, int season, IDbConnection connection);
        Task InsertMultipleAsync(List<Team> teams, IDbConnection connection);
    }
}
