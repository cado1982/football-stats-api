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
                var subscription = await connection.QuerySingleOrDefaultAsync<Subscription>(SubscriptionSql.GetByName, new { Name = name });

                return subscription;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get subscription with name {0}", name);
                throw;
            }
        }

        public async Task<Subscription> GetByIdAsync(int id, IDbConnection connection)
        {
            try
            {
                var subscription = await connection.QuerySingleOrDefaultAsync<Subscription>(SubscriptionSql.GetById, new { Id = id });

                return subscription;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get subscription with id {0}", id);
                throw;
            }
        }

        public async Task<List<Subscription>> GetAllAsync(IDbConnection connection)
        {
            try
            {
                var lookup = new Dictionary<int, Subscription>();

                var subscriptions = await connection.QueryAsync<Subscription, SubscriptionFeature, Subscription>(SubscriptionSql.GetAll, (s, sf) => 
                {
                    if (!lookup.TryGetValue(s.Id, out var found))
                    {
                        lookup.Add(s.Id, found = s);
                        found.Features = new List<SubscriptionFeature>();
                    }
                    found.Features.Add(sf);
                    return found;
                });

                return lookup.Values.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get subscriptions");
                throw;
            }
        }

        public async Task ChangeUsersSubscription(int userId, int newSubscriptionId, IDbConnection connection)
        {
            try
            {
                await connection.ExecuteAsync(SubscriptionSql.ChangeUsersSubscription, new { UserId = userId, SubscriptionId = newSubscriptionId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to change user {0} to subscription {1}", userId, newSubscriptionId);
                throw;
            }
        }
    }
}
