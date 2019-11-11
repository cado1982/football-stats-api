using FootballStatsApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace FootballStatsApi.Domain.Repositories
{
    public interface IPlayerSummaryRepository
    {
        Task<List<PlayerSummary>> GetAsync(IDbConnection connection);
    }
}
