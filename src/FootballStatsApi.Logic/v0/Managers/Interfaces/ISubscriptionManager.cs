using FootballStatsApi.Models.v0;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballStatsApi.Logic.v0.Managers
{
    public interface ISubscriptionManager
    {
        Task<Subscription> GetSubscriptionByName(string name);
        Task<Subscription> GetSubscriptionById(int id);
        Task<List<Subscription>> GetAllSubscriptions();
        Task ChangeUsersSubscription(int userId, int newSubscriptionId);
    }
}