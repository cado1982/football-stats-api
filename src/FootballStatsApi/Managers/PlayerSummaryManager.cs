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
                using (var conn = _connectionProvider.GetOpenConnection())
                {
                    var competition = await _competitionRepository.GetByIdAsync(competitionId, conn);

                    if (competition == null)
                    {
                        throw new ApplicationException($"Competition {competitionId} not found");
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
    }
}
