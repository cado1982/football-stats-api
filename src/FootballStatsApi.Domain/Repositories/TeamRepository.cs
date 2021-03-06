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
    public class TeamRepository : ITeamRepository
    {
        private readonly ILogger<TeamRepository> _logger;

        public TeamRepository(ILogger<TeamRepository> logger)
        {
            _logger = logger;
        }

        public async Task<List<Team>> GetAllTeamsAsync(IDbConnection connection)
        {
            try
            {
                var result = await connection.QueryAsync<Team>(TeamSql.GetAll);
                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get teams");
                throw;
            }
        }

        //public async Task<List<TeamBasicStats>> GetBasicStatsAsync(int competitionId, int season, IDbConnection connection)
        //{
        //    try
        //    {
        //        var parameters = new DynamicParameters();

        //        parameters.Add("CompetitionId", competitionId);
        //        parameters.Add("SeasonId", season);

        //        var result = await connection.QueryAsync<TeamBasicStats, Team, TeamBasicStats>(TeamSql.GetBasicStatsByCompetitionAndSeason, (ts, t) => {
        //            ts.Team = t;
        //            return ts;                
        //        }, parameters);
        //        return result.ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Unable to get basic stats for teams in competition {0} and season {1}", competitionId, season);
        //        throw;
        //    }
        //}

        public async Task<List<Team>> GetTeamsAsync(int competitionId, int season, IDbConnection connection)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("CompetitionId", competitionId);
                parameters.Add("SeasonId", season);

                var result = await connection.QueryAsync<Team>(TeamSql.GetByCompetitionAndSeason, parameters);
                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get teams for competition {0} and season {1}", competitionId, season);
                throw;
            }
        }

        public async Task InsertMultipleAsync(List<Team> teams, IDbConnection connection)
        {
            try
            {
                await connection.ExecuteAsync(TeamSql.InsertMultiple, teams);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to insert teams");
                throw;
            }
        }
    }
}
