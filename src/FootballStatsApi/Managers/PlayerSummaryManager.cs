using FootballStatsApi.Domain.Helpers;
using FootballStatsApi.Domain.Repositories;
using FootballStatsApi.Extensions;
using FootballStatsApi.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballStatsApi.Managers
{
    public class PlayerSummaryManager : IPlayerSummaryManager
    {
        private readonly ILogger<PlayerSummaryManager> _logger;
        private readonly IPlayerSummaryRepository _playerSummaryRepository;
        private readonly IConnectionProvider _connectionProvider;

        public PlayerSummaryManager(
            ILogger<PlayerSummaryManager> logger, 
            IPlayerSummaryRepository playerSummaryRepository,
            IConnectionProvider connectionProvider)
        {
            _logger = logger;
            _playerSummaryRepository = playerSummaryRepository;
            _connectionProvider = connectionProvider;
        }

        public async Task<List<PlayerSummary>> GetAsync()
        {
            try
            {
                using (var conn = _connectionProvider.GetOpenConnection())
                {
                    var entities = await _playerSummaryRepository.GetAsync(conn);
                    var models = entities.ToModels().ToList();

                    return models;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get player summaries");
                throw;
            }
        }
    }
}
