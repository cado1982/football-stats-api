using FootballStatsApi.Domain.Helpers;
using FootballStatsApi.Domain.Repositories;
using FootballStatsApi.Logic.Extensions;
using FootballStatsApi.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FootballStatsApi.Logic.Managers
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

        public async Task<TeamSummaries> GetAsync(int season, int competitionId)
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

                    var entities = await _teamSummaryRepository.GetAsync(season, competitionId, conn);
                    var summaries = entities.ToModels().ToList();

                    return new TeamSummaries
                    {
                        Season = season,
                        Competition = competition.ToModel(),
                        Teams = summaries
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get team summaries for season {season} and competition {competitionId}");
                throw;
            }
        }

        public async Task<TeamSummary> GetByIdAsync(int teamId, int season, int competitionId)
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

                    var entity = await _teamSummaryRepository.GetByIdAsync(teamId, season, competitionId, conn);
                    var model = entity.ToModel();

                    return model;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get team summaries for team {teamId} and season {season} and competition {competitionId}");
                throw;
            }
        }
    }
}
