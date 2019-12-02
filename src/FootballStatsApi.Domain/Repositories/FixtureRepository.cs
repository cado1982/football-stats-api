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
    public class FixtureRepository : IFixtureRepository
    {
        private readonly ILogger<FixtureRepository> _logger;

        public FixtureRepository(ILogger<FixtureRepository> logger)
        {
            _logger = logger;
        }

        public async Task<FixtureDetails> GetFixtureDetailsAsync(int fixtureId, IDbConnection connection)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@FixtureId", fixtureId);

                var players = await connection.QueryAsync<FixtureDetails, Competition, Team, Team, FixtureDetails>(FixtureSql.GetDetails, (fd, c, ht, at) =>
                {
                    fd.Competition = c;
                    fd.HomeTeam = ht;
                    fd.AwayTeam = at;
                    return fd;
                }, parameters);

                return players.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get fixture details for fixture {fixtureId}");
                throw;
            }
        }

        public async Task<List<FixturePlayer>> GetFixturePlayersAsync(int fixtureId, IDbConnection connection)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@FixtureId", fixtureId);

                var players = await connection.QueryAsync<FixturePlayer, Player, Player, Player, Team, FixturePlayer>(FixtureSql.GetFixturePlayers, (fp, p, rb, r, t) =>
                {
                    fp.Player = p;
                    fp.ReplacedBy = rb;
                    fp.Replaced = r;
                    fp.Team = t;
                    return fp;
                }, parameters);

                return players.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get fixture players for fixture {fixtureId}");
                throw;
            }
        }

        public async Task<List<FixtureShot>> GetFixtureShotsAsync(int fixtureId, IDbConnection connection)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@FixtureId", fixtureId);

                var shots = await connection.QueryAsync<FixtureShot, Player, Player, Team,FixtureShot>(FixtureSql.GetFixtureShots, (fs, p, ap, t) =>
                {
                    fs.Player = p;
                    fs.Assist = ap;
                    fs.Team = t;
                    return fs;
                }, parameters);

                return shots.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get fixture shots for fixture {fixtureId}");
                throw;
            }
        }

        public async Task<List<int>> GetFixturesToCheckAsync(IDbConnection connection)
        {
            try
            {
                var shots = await connection.QueryAsync<int>(FixtureSql.GetFixtureIdsToCheck);

                return shots.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get recently ended fixtures");
                throw;
            }
        }

        public async Task<List<int>> InsertMultipleAsync(List<FixtureDetails> fixtures, IDbConnection connection)
        {
            try
            {
                var ids = await connection.QueryAsync<int>(FixtureSql.InsertMultiple, fixtures.Select(f => new
                {
                    FixtureId = f.FixtureId,
                    HomeTeamId = f.HomeTeam.Id,
                    AwayTeamId = f.AwayTeam.Id,
                    SeasonId = f.Season,
                    CompetitionId = f.Competition.Id,
                    IsResult = f.IsResult,
                    HomeGoals = f.HomeGoals,
                    AwayGoals = f.AwayGoals,
                    ExpectedHomeGoals = f.HomeExpectedGoals,
                    ExpectedAwayGoals = f.AwayExpectedGoals,
                    HomeWinForecast = f.ForecastHomeWin,
                    DrawForecast = f.ForecastDraw,
                    AwayWinForecast = f.ForecastAwayWin,
                    DateTime = f.DateTime,
                    HomePasses = f.HomePasses,
                    AwayPasses = f.AwayPasses,
                    HomeDefensiveActions = f.HomeDefensiveActions,
                    AwayDefensiveActions = f.AwayDefensiveActions
                }));

                return ids.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to insert multiple fixtures");
                throw;
            }
        }

        public async Task<bool> IsFixtureSavedAsync(int fixtureId, IDbConnection connection)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@FixtureId", fixtureId);

                var alreadySaved = await connection.ExecuteScalarAsync<bool>(FixtureSql.IsFixtureSaved, parameters);
                return alreadySaved;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get fixture already saved status for fixture {fixtureId}");
                throw;
            }
        }

        public async Task UpdateDetailsSavedAsync(int fixtureId, IDbConnection connection)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@FixtureId", fixtureId);

                await connection.ExecuteAsync(FixtureSql.UpdateDetailsSaved, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to update details saved column for fixture {fixtureId}");
                throw;
            }
        }
    }
}
