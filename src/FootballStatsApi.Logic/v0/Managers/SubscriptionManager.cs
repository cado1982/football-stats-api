using FootballStatsApi.Domain.Helpers;
using FootballStatsApi.Domain.Repositories;
using FootballStatsApi.Logic.v0.Extensions;
using FootballStatsApi.Models.v0;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballStatsApi.Logic.v0.Managers
{
    public class SubscriptionManager : ISubscriptionManager
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IConnectionProvider _connectionProvider;
        private readonly IMemoryCache _cache;
        private static string _subscriptionsCacheKey = "cache://subscriptions";

        public SubscriptionManager(ISubscriptionRepository subscriptionRepository, IConnectionProvider connectionProvider, IMemoryCache cache)
        {
            _subscriptionRepository = subscriptionRepository;
            _connectionProvider = connectionProvider;
            _cache = cache;
        }

        public async Task<Subscription> GetSubscriptionByName(string name)
        {
            using (var conn = await _connectionProvider.GetOpenConnectionAsync())
            {
                var subscription = await _subscriptionRepository.GetByNameAsync(name, conn);

                return subscription.ToModel();
            }
        }

        public async Task<Subscription> GetSubscriptionById(int id)
        {
            using (var conn = await _connectionProvider.GetOpenConnectionAsync())
            {
                var subscription = await _subscriptionRepository.GetByIdAsync(id, conn);

                return subscription.ToModel();
            }
        }

        public async Task<List<Subscription>> GetAllSubscriptions()
        {
            if (!_cache.TryGetValue<List<Subscription>>(_subscriptionsCacheKey, out var models))
            {
                using (var conn = await _connectionProvider.GetOpenConnectionAsync())
                {
                    var entities = await _subscriptionRepository.GetAllAsync(conn);
                    models = entities.ToModels().ToList();

                    _cache.Set(_subscriptionsCacheKey, models, TimeSpan.FromHours(1));
                }
            }

            return models;
        }

        public async Task ChangeUsersSubscription(int userId, int newSubscriptionId)
        {
            using (var conn = await _connectionProvider.GetOpenConnectionAsync())
            {
                await _subscriptionRepository.ChangeUsersSubscription(userId, newSubscriptionId, conn);
            }
        }
    }
}
