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
    public class PlayerSummaryRepository : IPlayerSummaryRepository
    {
        private readonly ILogger<PlayerSummaryRepository> _logger;

        public PlayerSummaryRepository(ILogger<PlayerSummaryRepository> logger)
        {
            _logger = logger;
        }

        public async Task<List<PlayerSummary>> GetAsync(IDbConnection connection)
        {
            try
            {
                var result = await connection.QueryAsync<PlayerSummary>(PlayerSummarySql.Get);
                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get player summaries");
                throw;
            }
        }
    }
}
