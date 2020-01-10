using FootballStatsApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballStatsApi.Logic.Managers
{
    public interface ISubscriptionManager
    {
        Task<Subscription> GetSubscriptionByName(string name);
        Task<Subscription> GetSubscriptionById(int id);
        Task<List<Subscription>> GetAllSubscriptions();
        Task ChangeUsersSubscription(int userId, int newSubscriptionId);
    }
}