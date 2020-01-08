using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using FootballStatsApi.Domain.Entities;
using Microsoft.Extensions.Logging;
using Dapper;
using FootballStatsApi.Domain.Sql;

namespace FootballStatsApi.Domain.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly ILogger<SubscriptionRepository> _logger;

        public SubscriptionRepository(ILogger<SubscriptionRepository> logger)
        {
            _logger = logger;
        }

        public async Task<Subscription> GetByNameAsync(string name, IDbConnection connection)
        {
            try
            {
                return await connection.QuerySingleOrDefaultAsync<Subscription>(SubscriptionSql.GetByName, new { Name = name });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get subscription with name {0}", name);
                throw;
            }
        }
    }
}
