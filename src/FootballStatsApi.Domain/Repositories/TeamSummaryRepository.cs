using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using FootballStatsApi.Domain.Entities;
using Microsoft.Extensions.Logging;
using Dapper;
using FootballStatsApi.Domain.Sql;
using System.Linq;

namespace FootballStatsApi.Domain.Repositories
{
    public class TeamSummaryRepository : ITeamSummaryRepository
    {
        private readonly ILogger<TeamSummaryRepository> _logger;

        public TeamSummaryRepository(ILogger<TeamSummaryRepository> logger)
        {
            _logger = logger;
        }

        public async Task<List<TeamSummary>> GetAsync(int seasonId, IDbConnection connection)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@SeasonId", seasonId);

                var result = await connection.QueryAsync<TeamSummary>(TeamSummarySql.Get, parameters);
                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get team summaries");
                throw;
            }
        }
    }
}
