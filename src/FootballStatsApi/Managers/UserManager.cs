using System;
using System.Linq;
using System.Threading.Tasks;
using FootballStatsApi.Domain;
using FootballStatsApi.Domain.Entities.Identity;
using Microsoft.Extensions.Logging;

namespace FootballStatsApi.Managers
{
    public class UserManager : IUserManager
    {
        private readonly ILogger<UserManager> _logger;
        private readonly ApplicationDbContext _dbContext;

        public UserManager(
            ILogger<UserManager> logger, 
            ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public Task<User> GetUserByApiKeyAsync(Guid apiKey)
        {
            if (apiKey == Guid.Empty) 
            {
                throw new ArgumentException("apiKey must be provided", nameof(apiKey));
            }

            return Task.FromResult(_dbContext.Users.SingleOrDefault(u => u.ApiKey == apiKey));
        }
    }
}
