using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballStatsApi.Domain;
using FootballStatsApi.Domain.Entities.Identity;
using FootballStatsApi.Domain.Helpers;
using FootballStatsApi.Domain.Repositories;
using FootballStatsApi.Extensions;
using FootballStatsApi.Models;
using Microsoft.Extensions.Logging;

namespace FootballStatsApi.Managers
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

        public async Task<FixtureDetails> GetDetailsAsync(int fixtureId)
        {
            try
            {
                using (var conn = _connectionProvider.GetOpenConnection())
                {
                    var details = await _fixtureRepository.GetFixtureDetailsAsync(fixtureId, conn);
                    //var shots = await _fixtureRepository.GetFixtureShotsAsync(fixtureId, conn);

                    return details.ToModel();
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
