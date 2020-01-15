using FootballStatsApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace FootballStatsApi.Domain.Repositories
{
    public interface ITeamSummaryRepository
    {
        Task<List<TeamSummary>> GetBySeasonAndCompetitionAsync(int seasonId, int competitionId, IDbConnection connection);
        Task<TeamSummary> GetByTeamIdAndSeasonAsync(int teamId, int seasonId, IDbConnection connection);
        Task<List<TeamSummary>> GetByTeamIdAsync(int teamId, IDbConnection connection);
        Task InsertMultipleAsync(List<TeamSummary> summaries, IDbConnection connection);
    }
}
