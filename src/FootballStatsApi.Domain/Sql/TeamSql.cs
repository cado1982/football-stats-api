using System;
using System.Collections.Generic;
using System.Text;

namespace FootballStatsApi.Domain.Sql
{
    public static class TeamSql
    {
        public static string GetAll = @"SELECT id, name, short_name as shortname FROM ""stats"".""team""";
        public static string InsertMultiple = @"INSERT INTO ""stats"".""team"" (id, name, short_name) VALUES (@Id, @Name, @ShortName) ON CONFLICT(id) DO NOTHING;";
    }
}
