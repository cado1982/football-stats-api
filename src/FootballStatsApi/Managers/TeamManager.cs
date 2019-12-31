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
    public class TeamManager : ITeamManager
    {
        private readonly ILogger<TeamManager> _logger;
        private readonly ITeamRepository _teamRepository;
        private readonly IConnectionProvider _connectionProvider;

        public TeamManager(
            ILogger<TeamManager> logger, 
            ITeamRepository teamRepository,
            IConnectionProvider connectionProvider)
        {
            _logger = logger;
            _teamRepository = teamRepository;
            _connectionProvider = connectionProvider;
        }

        public async Task<List<Team>> GetAsync()
        {
            try
            {
                using (var conn = await _connectionProvider.GetOpenConnectionAsync())
                {
                    var entities = await _teamRepository.GetAllTeamsAsync(conn);
                    var models = entities.ToModels().ToList();

                    return models;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get all teams");
                throw;
            }
        }

        public async Task<List<Team>> GetAsync(int season, int competitionId)
        {
            try
            {
                using (var conn = await _connectionProvider.GetOpenConnectionAsync())
                {
                    var entities = await _teamRepository.GetTeamsAsync(competitionId, season, conn);
                    var models = entities.ToModels().ToList();

                    return models;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get teams for competition {0} and season {1}", competitionId, season);
                throw;
            }
        }
    }
}
