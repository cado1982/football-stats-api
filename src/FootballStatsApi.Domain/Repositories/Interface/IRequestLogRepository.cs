using FootballStatsApi.Domain.Entities;
using System.Data;
using System.Threading.Tasks;

namespace FootballStatsApi.Domain.Repositories
{
    public interface IRequestLogRepository
    {
        Task<RateLimitStatus> GetRateLimitInfoByUser(int userId, IDbConnection connection);
        Task InsertRequestLog(RequestLog requestLog, IDbConnection connection);
    }
}
