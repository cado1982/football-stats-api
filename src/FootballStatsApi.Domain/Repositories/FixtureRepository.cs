﻿using System;
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

        public async Task InsertFixturePlayers(List<FixturePlayer> players, int fixtureId, IDbConnection connection)
        {
            try
            {
                if (players.Any(p => p.Player == null)) { throw new ArgumentException("Each FixturePlayer must have its Player property set."); }
                if (players.Any(p => p.Team == null)) { throw new ArgumentException("Each FixturePlayer must have its Team property set."); }

                await connection.ExecuteAsync(FixtureSql.InsertFixturePlayers, players.Select(p => new 
                {
                    FixtureId = fixtureId,
                    PlayerId = p.Player.Id,
                    TeamId = p.Team.Id,
                    Time = p.Minutes,
                    Position = p.Position,
                    YellowCards = p.YellowCards,
                    RedCards = p.RedCards,
                    ReplacedById = p.ReplacedBy?.Id,
                    ReplacedId = p.Replaced?.Id,
                    KeyPasses = p.KeyPasses,
                    Assists = p.Assists,
                    ExpectedGoalsChain = p.ExpectedGoalsChain,
                    ExpectedGoalsBuildup = p.ExpectedGoalsBuildup,
                    PositionOrder = p.PositionOrder,
                    Goals = p.Goals,
                    OwnGoals = p.OwnGoals,
                    Shots = p.Shots,
                    ExpectedGoals = p.ExpectedGoals,
                    ExpectedAssists = p.ExpectedAssists
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to insert fixture players for fixture {fixtureId}");
                throw;
            }
        }

        public async Task InsertFixtureShots(List<FixtureShot> shots, int fixtureId, IDbConnection connection)
        {
            try
            {
                if (shots.Any(f => f.Player == null)) { throw new ArgumentException("Each FixtureShot must have its Player property set."); }
                if (shots.Any(f => f.Team == null)) { throw new ArgumentException("Each FixtureShot must have its Team property set."); }

                await connection.ExecuteAsync(FixtureSql.InsertFixtureShots, shots.Select(f => new
                {
                    ShotId = f.ShotId,
                    PlayerId = f.Player.Id,
                    FixtureId = fixtureId,
                    TeamId = f.Team.Id,
                    Minute = f.Minute,
                    Result = f.Result,
                    X = f.X,
                    Y = f.Y,
                    ExpectedGoal = f.ExpectedGoal,
                    Situation = f.Situation,
                    ShotType = f.Type,
                    LastAction = f.LastAction,
                    AssistedById = f.Assist?.Id
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to insert fixture shots for fixture {fixtureId}");
                throw;
            }
        }

        public async Task InsertMultipleAsync(List<FixtureDetails> fixtures, IDbConnection connection)
        {
            try
            {
                if (fixtures.Any(f => f.HomeTeam == null)) { throw new ArgumentException("Each FixtureDetails must have its HomeTeam property set."); }
                if (fixtures.Any(f => f.AwayTeam == null)) { throw new ArgumentException("Each FixtureDetails must have its AwayTeam property set."); }
                if (fixtures.Any(f => f.Competition == null)) { throw new ArgumentException("Each FixtureDetails must have its Competition property set."); }

                await connection.ExecuteAsync(FixtureSql.InsertMultiple, fixtures.Select(f => new
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
                    HomePpda = f.HomePpda,
                    AwayPpda = f.AwayPpda
                }));
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

        public async Task Update(FixtureDetails fixture, IDbConnection connection)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@FixtureId", fixture.FixtureId);
                parameters.Add("@HomeDeep", fixture.HomeDeepPasses);
                parameters.Add("@AwayDeep", fixture.AwayDeepPasses);
                parameters.Add("@ExpectedHomeGoals", fixture.HomeExpectedGoals);
                parameters.Add("@ExpectedAwayGoals", fixture.AwayExpectedGoals);
                parameters.Add("@HomeGoals", fixture.HomeGoals);
                parameters.Add("@AwayGoals", fixture.AwayGoals);
                parameters.Add("@HomeShots", fixture.HomeShots);
                parameters.Add("@AwayShots", fixture.AwayShots);
                parameters.Add("@HomeWinForecast", fixture.ForecastHomeWin);
                parameters.Add("@DrawForecast", fixture.ForecastDraw);
                parameters.Add("@AwayWinForecast", fixture.ForecastAwayWin);
                parameters.Add("@HomeShotsOnTarget", fixture.HomeShotsOnTarget);
                parameters.Add("@AwayShotsOnTarget", fixture.AwayShotsOnTarget);
                parameters.Add("@HomePpda", fixture.HomePpda);
                parameters.Add("@AwayPpda", fixture.AwayPpda);

                await connection.ExecuteAsync(FixtureSql.Update, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to update fixture {fixture.FixtureId}");
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
