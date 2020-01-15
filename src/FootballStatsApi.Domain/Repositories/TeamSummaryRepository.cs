using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using FootballStatsApi.Domain.Entities;
using Microsoft.Extensions.Logging;
using Dapper;
using FootballStatsApi.Domain.Sql;
using System.Linq;

namespace FootballStatsApi.Domain.Repositories
{
    public class TeamSummaryRepository : ITeamSummaryRepository
    {
        private readonly ILogger<TeamSummaryRepository> _logger;

        public TeamSummaryRepository(ILogger<TeamSummaryRepository> logger)
        {
            _logger = logger;
        }

        public async Task<List<TeamSummary>> GetBySeasonAndCompetitionAsync(int seasonId, int competitionId, IDbConnection connection)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@SeasonId", seasonId);
                parameters.Add("@CompetitionId", competitionId);

                var result = await connection.QueryAsync<TeamSummary, Team, TeamSummary>(TeamSummarySql.GetBySeasonAndCompetition, (ts, t) => {
                    ts.Team = t;
                    return ts;
                }, parameters);
                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get team summaries for season {seasonId} and competition {competitionId}");
                throw;
            }
        }

        public async Task<TeamSummary> GetByTeamIdAndSeasonAsync(int teamId, int seasonId, IDbConnection connection)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@TeamId", teamId);
                parameters.Add("@SeasonId", seasonId);

                var result = await connection.QueryAsync<TeamSummary, Team, TeamSummary>(TeamSummarySql.GetByTeamIdAndSeason, (ts, t) => {
                    ts.Team = t;
                    return ts;
                }, parameters);

                var teamSummary = result.FirstOrDefault();
                return teamSummary;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get team summaries for team {0} season {1}.", teamId, seasonId);
                throw;
            }
        }

        public async Task<List<TeamSummary>> GetByTeamIdAsync(int teamId, IDbConnection connection)
        {
            try
            {
                var result = await connection.QueryAsync<TeamSummary, Team, TeamSummary>(TeamSummarySql.GetByTeamId, (ts, t) => {
                    ts.Team = t;
                    return ts;
                }, new { TeamId = teamId });

                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get team summaries for team {0}.", teamId);
                throw;
            }
        }

        public async Task InsertMultipleAsync(List<TeamSummary> summaries, IDbConnection connection)
        {
            try
            {
                await connection.ExecuteAsync(TeamSummarySql.InsertMultiple, summaries.Select(s => new 
                {
                    TeamId = s.Team.Id,
                    Season = s.Season,
                    CompetitionId = s.Competition.Id,
                    Games = s.Games,
                    Won = s.Won,
                    Drawn = s.Drawn,
                    Lost = s.Lost,
                    GoalsFor = s.GoalsFor,
                    GoalsAgainst = s.GoalsAgainst,
                    Points = s.Points,
                    ExpectedGoals = s.ExpectedGoals,
                    NonPenaltyExpectedGoals = s.NonPenaltyExpectedGoals,
                    ExpectedGoalsAgainst = s.ExpectedGoalsAgainst,
                    NonPenaltyExpectedGoalsAgainst = s.NonPenaltyExpectedGoalsAgainst,
                    ExpectedPoints = s.ExpectedPoints,
                    Ppda = s.Ppda,
                    OppositionPpda = s.OppositionPpda,
                    DeepPasses = s.DeepPasses,
                    OppositionDeepPasses = s.OppositionDeepPasses
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to insert team summaries");
                throw;
            }
        }
    }
}
