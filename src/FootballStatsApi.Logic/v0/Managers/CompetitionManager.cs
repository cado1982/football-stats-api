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
    public class CompetitionManager : ICompetitionManager
    {
        private readonly ILogger<CompetitionManager> _logger;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly IConnectionProvider _connectionProvider;

        public CompetitionManager(
            ILogger<CompetitionManager> logger,
            ICompetitionRepository competitionRepository,
            IConnectionProvider connectionProvider)
        {
            _logger = logger;
            _competitionRepository = competitionRepository;
            _connectionProvider = connectionProvider;
        }

        public async Task<List<Competition>> GetAsync()
        {
            try
            {
                using (var conn = await _connectionProvider.GetOpenConnectionAsync())
                {
                    var entities = await _competitionRepository.GetAsync(conn);
                    var models = entities.ToModels().ToList();

                    return models;
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
