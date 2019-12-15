using Dapper;
using FootballStatsApi.Domain.Entities;
using FootballStatsApi.Domain.Repositories.Interface;
using FootballStatsApi.Domain.Sql;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace FootballStatsApi.Domain.Repositories
{
    public class RateLimitRepository : IRateLimitRepository
    {
        private readonly ILogger<RateLimitRepository> _logger;

        public RateLimitRepository(ILogger<RateLimitRepository> logger)
        {
            _logger = logger;
        }

        public async Task<RateLimitStatus> GetRateLimitByApiKey(Guid apiKey, IDbConnection connection)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@ApiKey", apiKey);

                var result = await connection.QuerySingleAsync<RateLimitStatus>(RateLimitSql.Get, parameters);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to rate limit info for apiKey {0}", apiKey);
                throw;
            }
        }

        public async Task InsertRequestLog(RequestLog requestLog, IDbConnection connection)
        {
            try
            {
                await connection.ExecuteAsync(RateLimitSql.Insert, requestLog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to insert request log");
                throw;
            }
        }
    }
}
