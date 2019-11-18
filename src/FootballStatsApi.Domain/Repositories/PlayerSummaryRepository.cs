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

        public async Task<PlayerSummary> GetByIdAsync(int playerId, int seasonId, int competitionId, IDbConnection connection)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@PlayerId", playerId);
                parameters.Add("@SeasonId", seasonId);
                parameters.Add("@CompetitionId", competitionId);

                var result = await connection.QueryAsync<PlayerSummary, Player, Team, PlayerSummary>(PlayerSummarySql.Get, (ps, p, t) => {
                    ps.Player = p;
                    ps.Team = t;
                    return ps;
                }, parameters);

                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get player summary for player {playerId} and season {seasonId} and competition {competitionId}");
                throw;
            }
        }
    }
}
