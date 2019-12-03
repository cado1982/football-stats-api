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
        Task<List<int>> GetFixturesToCheckAsync(IDbConnection connection);
        Task<bool> IsFixtureSavedAsync(int fixtureId, IDbConnection connection);
        Task InsertMultipleAsync(List<FixtureDetails> fixtures, IDbConnection connection);
        Task UpdateDetailsSavedAsync(int fixtureId, IDbConnection connection);
        Task Update(FixtureDetails fixture, IDbConnection connection);
        Task InsertFixturePlayers(List<FixturePlayer> players, int fixtureId, IDbConnection connection);
        Task InsertFixtureShots(List<FixtureShot> shots, int fixtureId, IDbConnection connection);
    }
}
