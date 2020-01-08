using FootballStatsApi.Domain.Entities.Identity;
using System;
using System.Threading.Tasks;

namespace FootballStatsApi.Logic.Managers
{
    public interface IUserManager
    {
        Task<User> GetUserByApiKeyAsync(Guid apiKey);
    }
}
