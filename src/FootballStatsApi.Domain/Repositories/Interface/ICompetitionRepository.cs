using FootballStatsApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace FootballStatsApi.Domain.Repositories
{
    public interface ICompetitionRepository
    {
        Task<List<Competition>> GetAsync(IDbConnection connection);
    }
}
