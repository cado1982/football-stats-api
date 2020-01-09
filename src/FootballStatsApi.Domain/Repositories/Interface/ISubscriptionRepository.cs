using FootballStatsApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace FootballStatsApi.Domain.Repositories
{
    public interface ISubscriptionRepository
    {
        Task<Subscription> GetByNameAsync(string name, IDbConnection connection);
        Task<Subscription> GetByIdAsync(int id, IDbConnection connection);
    }
}
