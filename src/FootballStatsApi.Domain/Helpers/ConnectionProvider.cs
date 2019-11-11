using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace FootballStatsApi.Domain.Helpers
{
    public class ConnectionProvider : IConnectionProvider
    {
        private readonly DatabaseConnectionInfo _connectionInfo;

        public ConnectionProvider(DatabaseConnectionInfo connectionInfo)
        {
            _connectionInfo = connectionInfo;
        }

        public IDbConnection GetConnection()
        {
            return new NpgsqlConnection(_connectionInfo.ConnectionString);
        }

        public IDbConnection GetOpenConnection()
        {
            var connection = GetConnection();
            connection.Open();
            return connection;
        }
    }
}
