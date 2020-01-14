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

        public async Task<Fixture> GetFixtureDetailsAsync(int fixtureId, IDbConnection connection)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@FixtureId", fixtureId);

                var players = await connection.QueryAsync<Fixture, Competition, Team, Team, Fixture>(FixtureSql.GetDetails, (fd, c, ht, at) =>
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

        public async Task<List<FixturePlayer>> GetFixturePlayersByCompetitionAndSeasonAsync(int competitionId, int season, IDbConnection connection)
        {
            try
            {
                var parameters = new
                {
                    CompetitionId = competitionId,
                    SeasonId = season
                };

                var fixturePlayers = await connection.QueryAsync<FixturePlayer, Player, Team, Player, Player, FixturePlayer>(FixtureSql.GetPlayerDetailsForCompetitionAndSeason, (fpd, p, t, r, rb) =>
                {
                    fpd.Team = t;
                    fpd.Player = p;
                    fpd.Replaced = r;
                    fpd.ReplacedBy = rb;
                    return fpd;
                }, parameters);

                return fixturePlayers.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get fixture players for competition {0} and season {1}", competitionId, season);
                throw;
            }
        }

        public async Task<List<Fixture>> GetFixturesByCompetitionAndSeasonAsync(int competitionId, int season, IDbConnection connection)
        {
            try
            {
                var parameters = new
                {
                    CompetitionId = competitionId,
                    SeasonId = season
                };

                var fixtures = await connection.QueryAsync<Fixture, Competition, Team, Team, Fixture>(FixtureSql.GetAllDetailsForCompetitionAndSeason, (fd, c, ht, at) =>
                {
                    fd.Competition = c;
                    fd.HomeTeam = ht;
                    fd.AwayTeam = at;
                    return fd;
                }, parameters);

                return fixtures.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get fixture details for competition {0} and season {1}", competitionId, season);
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
        }testc the scraper to make sure it's working for the new stats

        public async Task InsertFixturePlayers(List<FixturePlayer> players, IDbConnection connection)
        {
            try
            {
                if (players.Any(p => p.Player == null)) { throw new ArgumentException("Each FixturePlayer must have its Player property set."); }
                if (players.Any(p => p.Team == null)) { throw new ArgumentException("Each FixturePlayer must have its Team property set."); }

                await connection.ExecuteAsync(FixtureSql.InsertFixturePlayers, players.Select(p => new
                {
                    PlayerId = p.Player.Id,
                    FixtureId = p.FixtureId,
                    TeamId = p.Team.Id,
                    Time = p.Minutes,
                    Position = p.Position,
                    YellowCards = p.YellowCards,
                    RedCards = p.RedCards,
                    ReplacedById = p.ReplacedBy?.Id,
                    ReplacedId = p.Replaced?.Id,
                    ExpectedGoalsChain = p.ExpectedGoalsChain,
                    ExpectedGoalsBuildup = p.ExpectedGoalsBuildup,
                    Shots = p.Shots,
                    ShotsOnTarget = p.ShotsOnTarget,
                    KeyPasses = p.KeyPasses,
                    Assists = p.Assists,
                    Goals = p.Goals,
                    OwnGoals = p.OwnGoals,
                    ExpectedGoals = p.ExpectedGoals,
                    ExpectedAssists = p.ExpectedAssists,
                    PositionOrder = p.PositionOrder
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to insert fixture players.");
                throw;
            }
        }

        public async Task InsertFixtureShots(List<FixtureShot> shots, IDbConnection connection)
        {
            try
            {
                if (shots.Any(f => f.Player == null)) { throw new ArgumentException("Each FixtureShot must have its Player property set."); }
                if (shots.Any(f => f.Team == null)) { throw new ArgumentException("Each FixtureShot must have its Team property set."); }

                await connection.ExecuteAsync(FixtureSql.InsertFixtureShots, shots.Select(s => new
                {
                    ShotId = s.ShotId,
                    PlayerId = s.Player.Id,
                    FixtureId = s.FixtureId,
                    TeamId = s.Team.Id,
                    Minute = s.Minute,
                    Result = s.Result,
                    X = s.X,
                    Y = s.Y,
                    ExpectedGoal = s.ExpectedGoal,
                    Situation = s.Situation,
                    ShotType = s.Type,
                    LastAction = s.LastAction,
                    AssistedById = s.Assist?.Id
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to insert fixture shots");
                throw;
            }
        }

        public async Task InsertMultipleAsync(List<Fixture> fixtures, IDbConnection connection)
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
                    DateTime = f.DateTime
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

        public async Task Update(Fixture fixture, IDbConnection connection)
        {
            try
            {
                //var parameters = new DynamicParameters();

                //parameters.Add("@FixtureId", fixture.FixtureId);
                //parameters.Add("@HomeDeep", fixture.HomeDeepPasses);
                //parameters.Add("@AwayDeep", fixture.AwayDeepPasses);
                //parameters.Add("@HomeWinForecast", fixture.HomeWinForecast);
                //parameters.Add("@DrawForecast", fixture.DrawForecast);
                //parameters.Add("@AwayWinForecast", fixture.AwayWinForecast);
                //parameters.Add("@HomePpda", fixture.HomePpda);
                //parameters.Add("@AwayPpda", fixture.AwayPpda);

                await connection.ExecuteAsync(FixtureSql.Update, fixture);
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
