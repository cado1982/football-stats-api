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
    }
}
