using FootballStatsApi.Domain.Helpers;
using FootballStatsApi.Domain.Repositories;
using FootballStatsApi.Logic.Extensions;
using FootballStatsApi.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FootballStatsApi.Logic.Managers
{
    public class FixtureManager : IFixtureManager
    {
        private readonly ILogger<FixtureManager> _logger;
        private readonly IFixtureRepository _fixtureRepository;
        private readonly IConnectionProvider _connectionProvider;

        public FixtureManager(
            ILogger<FixtureManager> logger,
            IFixtureRepository fixtureRepository,
            IConnectionProvider connectionProvider)
        {
            _logger = logger;
            _fixtureRepository = fixtureRepository;
            _connectionProvider = connectionProvider;
        }

        //public async Task<FixtureBasic> GetAsync(int? competitionId, int? season, int? teamId)
        //{
        //    try
        //    {
        //        using (var conn = await _connectionProvider.GetOpenConnectionAsync())
        //        {

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Unable to get fixtures for competition {competitionId}competitions");
        //        throw;
        //    }
        //}

        public async Task<FixtureBasic> GetDetailsAsync(int fixtureId)
        {
            try
            {
                using (var conn = await _connectionProvider.GetOpenConnectionAsync())
                {
                    var details = await _fixtureRepository.GetFixtureDetailsAsync(fixtureId, conn);
                    var shots = await _fixtureRepository.GetFixtureShotsAsync(fixtureId, conn);

                    return details.ToModel(shots);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get competitions");
                throw;
            }
        }
    }
}
