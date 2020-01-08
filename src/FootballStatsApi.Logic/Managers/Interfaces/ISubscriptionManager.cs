using FootballStatsApi.Models;
using System.Threading.Tasks;

namespace FootballStatsApi.Logic.Managers
{
    public interface ISubscriptionManager
    {
        Task<Subscription> GetSubscriptionByName(string name);
    }
}