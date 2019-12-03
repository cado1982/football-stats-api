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

        public async Task<PlayerSummaries> GetAsync(int season, int competitionId)
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
                    var summaries = entities.ToModels().ToList();

                    return new PlayerSummaries 
                    {
                        Season = season,
                        Competition = competition.ToModel(),
                        Players = summaries
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get player summaries");
                throw;
            }
        }

        public async Task<PlayerSummary> GetByIdAsync(int playerId, int season, int competitionId)
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

                    var entity = await _playerSummaryRepository.GetByIdAsync(playerId, season, competitionId, conn);
                    var model = entity.ToModel();
                    
                    return model;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get player summary for player {playerId} and season {season} and competition {competitionId}");
                throw;
            }
        }
    }
}
