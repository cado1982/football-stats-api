using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using FootballStatsApi.Domain.Entities;
using Microsoft.Extensions.Logging;
using Dapper;
using FootballStatsApi.Domain.Sql;
using System.Linq;

namespace FootballStatsApi.Domain.Repositories
{
    public class FixtureRepository : IFixtureRepository
    {
        private readonly ILogger<FixtureRepository> _logger;

        public FixtureRepository(ILogger<FixtureRepository> logger)
        {
            _logger = logger;
        }

        public async Task<FixtureDetails> GetFixtureDetailsAsync(int fixtureId, IDbConnection connection)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@FixtureId", fixtureId);

                var result = await connection.ExecuteScalarAsync<FixtureDetails>(FixtureSql.GetDetails, parameters);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get fixture details for fixture {fixtureId}");
                throw;
            }
        }

        public Task<List<FixturePlayer>> GetFixturePlayersAsync(int fixtureId, IDbConnection connection)
        {
            throw new NotImplementedException();
        }

        public Task<List<FixtureShot>> GetFixtureShotsAsync(int fixtureId, IDbConnection connection)
        {
            throw new NotImplementedException();
        }
    }
}
