using System;
using System.Collections.Generic;
using System.Text;

namespace FootballStatsApi.Domain.Sql
{
    public static class PlayerSql
    {
        public static string InsertMultiple = @"
            INSERT INTO ""stats"".""player"" (id, name) VALUES (@Id, @Name) ON CONFLICT(id) DO NOTHING;";
    }
}
