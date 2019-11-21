using FootballStatsApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace FootballStatsApi.Domain.Repositories
{
    public interface IFixtureRepository
    {
        Task<FixtureDetails> GetFixtureDetailsAsync(int fixtureId, IDbConnection connection);
        Task<List<FixtureShot>> GetFixtureShotsAsync(int fixtureId, IDbConnection connection);
        Task<List<FixturePlayer>> GetFixturePlayersAsync(int fixtureId, IDbConnection connection);
    }
}
