using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace FootballStatsApi.Domain.Helpers
{
    public class ConnectionProvider : IConnectionProvider
    {
        private readonly DatabaseConnectionInfo _connectionInfo;
        private readonly ILogger<ConnectionProvider> _logger;
        private int _connectionAttempts = 0;

        public ConnectionProvider(DatabaseConnectionInfo connectionInfo, ILogger<ConnectionProvider> logger)
        {
            _connectionInfo = connectionInfo;
            _logger = logger;
        }

        public async Task<IDbConnection> GetOpenConnectionAsync()
        {
            _connectionAttempts++;

            try
            {
                var connection = new NpgsqlConnection(_connectionInfo.ConnectionString);
                await connection.OpenAsync();

                _connectionAttempts = 0;

                return connection;    
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to connect to Postgres. Attempt {_connectionAttempts}.");
            }

            // This line will only hit if an exception occurs
            return await GetOpenConnectionAsync();
            
        }
    }
}
