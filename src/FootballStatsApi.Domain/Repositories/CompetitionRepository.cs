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
    public class CompetitionRepository : ICompetitionRepository
    {
        private readonly ILogger<CompetitionRepository> _logger;

        public CompetitionRepository(ILogger<CompetitionRepository> logger)
        {
            _logger = logger;
        }

        public async Task<List<Competition>> GetAsync(IDbConnection connection)
        {
            try
            {
                var result = await connection.QueryAsync<Competition>(CompetitionSql.Get);
                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get competitions");
                throw;
            }
        }

        public async Task<Competition> GetByIdAsync(int competitionId, IDbConnection connection)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@CompetitionId", competitionId);

                var result = await connection.QuerySingleOrDefaultAsync<Competition>(CompetitionSql.GetById, parameters);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get competition '{competitionId}'");
                throw;
            }
        }
    }
}
