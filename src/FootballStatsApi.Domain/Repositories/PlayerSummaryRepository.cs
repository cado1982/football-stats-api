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
    public class PlayerSummaryRepository : IPlayerSummaryRepository
    {
        private readonly ILogger<PlayerSummaryRepository> _logger;

        public PlayerSummaryRepository(ILogger<PlayerSummaryRepository> logger)
        {
            _logger = logger;
        }

        public async Task<List<PlayerSummary>> GetAsync(int seasonId, int competitionId, IDbConnection connection)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@SeasonId", seasonId);
                parameters.Add("@CompetitionId", competitionId);

                var result = await connection.QueryAsync<PlayerSummary, Player, Team, PlayerSummary>(PlayerSummarySql.Get, (ps, p, t) => {
                    ps.Player = p;
                    ps.Team = t;
                    return ps;
                }, parameters);
                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get player summaries");
                throw;
            }
        }

        public async Task<PlayerSummary> GetByIdAsync(int playerId, int seasonId, IDbConnection connection)
        {
            try
            {
                var result = await connection.QueryAsync<PlayerSummary, Player, Team, PlayerSummary>(PlayerSummarySql.GetById, (ps, p, t) => {
                    ps.Player = p;
                    ps.Team = t;
                    return ps;
                }, new
                {
                    PlayerId = playerId,
                    SeasonId = seasonId
                });

                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get player summary for player {0} and season {1}", playerId, seasonId);
                throw;
            }
        }

        public async Task InsertPlayerSummariesAsync(List<PlayerSummary> summaries, IDbConnection connection)
        {
            try
            {
                await connection.ExecuteAsync(PlayerSummarySql.InsertMultiple, summaries.Select(s => new 
                {
                    PlayerId = s.Player.Id,
                    s.Assists,
                    CompetitionId = s.Competition.Id,
                    s.ExpectedAssists,
                    s.ExpectedGoals,
                    s.ExpectedGoalsBuildup,
                    s.ExpectedGoalsChain,
                    s.Games,
                    s.Goals,
                    s.KeyPasses,
                    s.MinutesPlayed,
                    s.NonPenaltyExpectedGoals,
                    s.NonPenaltyGoals,
                    s.Position,
                    s.RedCards,
                    s.Season,
                    s.Shots,
                    TeamId = s.Team.Id,
                    s.YellowCards
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to insert player summaries");
                throw;
            }
        }
    }
}
