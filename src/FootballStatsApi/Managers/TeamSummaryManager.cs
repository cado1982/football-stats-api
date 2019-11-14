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
    public class TeamSummaryManager : ITeamSummaryManager
    {
        private readonly ILogger<TeamSummaryManager> _logger;
        private readonly ITeamSummaryRepository _teamSummaryRepository;
        private readonly IConnectionProvider _connectionProvider;

        public TeamSummaryManager(
            ILogger<TeamSummaryManager> logger, 
            ITeamSummaryRepository teamSummaryRepository,
            IConnectionProvider connectionProvider)
        {
            _logger = logger;
            _teamSummaryRepository = teamSummaryRepository;
            _connectionProvider = connectionProvider;
        }

        public async Task<TeamSummaries> GetAsync(int season, int competitionId)
        {
            try
            {
                using (var conn = _connectionProvider.GetOpenConnection())
                {
                    var entities = await _teamSummaryRepository.GetAsync(season, competitionId, conn);
                    var summaries = entities.ToModels().ToList();

                    return new TeamSummaries 
                    {
                        Season = season,
                        CompetitionId = competitionId,
                        Teams = summaries
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get team summaries");
                throw;
            }
        }
    }
}
