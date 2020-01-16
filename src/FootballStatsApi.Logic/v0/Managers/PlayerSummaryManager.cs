using FootballStatsApi.Domain.Helpers;
using FootballStatsApi.Domain.Repositories;
using FootballStatsApi.Logic.v0.Extensions;
using FootballStatsApi.Models.v0;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballStatsApi.Logic.v0.Managers
{
    public class PlayerSummaryManager : IPlayerSummaryManager
    {
        private readonly ILogger<PlayerSummaryManager> _logger;
        private readonly IPlayerSummaryRepository _playerSummaryRepository;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly IConnectionProvider _connectionProvider;

        public PlayerSummaryManager(
            ILogger<PlayerSummaryManager> logger,
            IPlayerSummaryRepository playerSummaryRepository,
            ICompetitionRepository competitionRepository,
            IConnectionProvider connectionProvider)
        {
            _logger = logger;
            _playerSummaryRepository = playerSummaryRepository;
            _competitionRepository = competitionRepository;
            _connectionProvider = connectionProvider;
        }

        public async Task<List<PlayerSummaryBasic>> GetAsync(int season, int competitionId)
        {
            try
            {
                using (var conn = await _connectionProvider.GetOpenConnectionAsync())
                {
                    var competition = await _competitionRepository.GetByIdAsync(competitionId, conn);

                    if (competition == null)
                    {
                        throw new ArgumentException($"Competition {competitionId} not found", "competition");
                    }

                    var entities = await _playerSummaryRepository.GetAsync(season, competitionId, conn);
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

        public async Task<PlayerSummaryBasic> GetByIdAsync(int playerId, int season)
        {
            try
            {
                using (var conn = await _connectionProvider.GetOpenConnectionAsync())
                {
                    var entity = await _playerSummaryRepository.GetByIdAsync(playerId, season, conn);
                    var model = entity.ToModel();

                    return model;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get player summary for player {0} and season {1}.", playerId, season);
                throw;
            }
        }
    }
}
