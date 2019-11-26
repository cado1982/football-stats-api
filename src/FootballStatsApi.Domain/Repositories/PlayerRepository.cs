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
    public class PlayerRepository : IPlayerRepository
    {
        private readonly ILogger<PlayerRepository> _logger;

        public PlayerRepository(ILogger<PlayerRepository> logger)
        {
            _logger = logger;
        }

        public async Task InsertPlayersAsync(List<Player> players, IDbConnection connection)
        {
            try
            {
                await connection.ExecuteAsync(PlayerSql.InsertMultiple, players);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to insert players");
                throw;
            }
        }
    }
}
