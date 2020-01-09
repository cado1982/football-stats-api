using FootballStatsApi.Domain.Helpers;
using FootballStatsApi.Domain.Repositories;
using FootballStatsApi.Logic.Extensions;
using FootballStatsApi.Models;
using System;
using System.Threading.Tasks;

namespace FootballStatsApi.Logic.Managers
{
    public class SubscriptionManager : ISubscriptionManager
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IConnectionProvider _connectionProvider;

        public SubscriptionManager(ISubscriptionRepository subscriptionRepository, IConnectionProvider connectionProvider)
        {
            _subscriptionRepository = subscriptionRepository;
            _connectionProvider = connectionProvider;
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
    }
}
