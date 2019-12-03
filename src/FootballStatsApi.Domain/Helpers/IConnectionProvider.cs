using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace FootballStatsApi.Domain.Helpers
{
    public interface IConnectionProvider
    {
        Task<IDbConnection> GetOpenConnectionAsync();
    }
}
