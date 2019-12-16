using Dapper;
using FootballStatsApi.Domain.Entities;
using FootballStatsApi.Domain.Sql;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace FootballStatsApi.Domain.Repositories
{
    public class RequestLogRepository : IRequestLogRepository
    {
        private readonly ILogger<RequestLogRepository> _logger;

        public RequestLogRepository(ILogger<RequestLogRepository> logger)
        {
            _logger = logger;
        }

        public async Task<RateLimitStatus> GetRateLimitInfoByUser(int userId, IDbConnection connection)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@UserId", userId);
                var result = await connection.QueryMultipleAsync(RequestLogSql.GetRateLimitInfo, parameters);

                var requestsThisInterval = await result.ReadSingleAsync<long>();
                var rateLimitStatus = await result.ReadSingleAsync<RateLimitStatus>();

                rateLimitStatus.RequestsThisInterval = requestsThisInterval;

                return rateLimitStatus;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to rate limit info for user {0}", userId);
                throw;
            }
        }

        public async Task InsertRequestLog(RequestLog requestLog, IDbConnection connection)
        {
            try
            {
                await connection.ExecuteAsync(RequestLogSql.Insert, requestLog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to insert request log");
                throw;
            }
        }
    }
}
