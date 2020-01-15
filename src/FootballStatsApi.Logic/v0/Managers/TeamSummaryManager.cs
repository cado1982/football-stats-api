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
    public class TeamSummaryManager : ITeamSummaryManager
    {
        private readonly ILogger<TeamSummaryManager> _logger;
        private readonly ITeamSummaryRepository _teamSummaryRepository;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly IConnectionProvider _connectionProvider;

        public TeamSummaryManager(
            ILogger<TeamSummaryManager> logger,
            ITeamSummaryRepository teamSummaryRepository,
            ICompetitionRepository competitionRepository,
            IConnectionProvider connectionProvider)
        {
            _logger = logger;
            _teamSummaryRepository = teamSummaryRepository;
            _competitionRepository = competitionRepository;
            _connectionProvider = connectionProvider;
        }

        public async Task<List<TeamSummaryBasic>> GetAsync(int season, int competitionId)
        {
            try
            {
                using (var conn = await _connectionProvider.GetOpenConnectionAsync())
                {
                    var competition = await _competitionRepository.GetByIdAsync(competitionId, conn);

                    if (competition == null)
                    {
                        throw new ApplicationException($"Competition {competitionId} not found");
                    }

                    var entities = await _teamSummaryRepository.GetBySeasonAndCompetitionAsync(season, competitionId, conn);
                    var models = entities.ToModelsBasic().ToList();

                    return models;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get team summaries for season {season} and competition {competitionId}");
                throw;
            }
        }

        public async Task<TeamSummaryBasic> GetByIdAsync(int teamId, int season)
        {
            try
            {
                using (var conn = await _connectionProvider.GetOpenConnectionAsync())
                {
                    var entity = await _teamSummaryRepository.GetByTeamIdAndSeasonAsync(teamId, season, conn);
                    var model = entity.ToModelBasic();

                    return model;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get team summaries for team {0} and season {1}.", teamId, season);
                throw;
            }
        }

        public async Task<List<TeamSummaryBasic>> GetByIdAsync(int teamId)
        {
            try
            {
                using (var conn = await _connectionProvider.GetOpenConnectionAsync())
                {
                    var entities = await _teamSummaryRepository.GetByTeamIdAsync(teamId, conn);
                    var models = entities.ToModelsBasic().ToList();

                    return models;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get team basic stats for all seasons for team {0}", teamId);
                throw;
            }
        }
    }
}
