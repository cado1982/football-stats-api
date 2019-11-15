using System;
using System.Collections.Generic;
using System.Text;

namespace FootballStatsApi.Domain.Sql
{
    public static class CompetitionSql
    {
        public static string Get = @"
        SELECT 
            id as competitionid,
            name,
            internal_name as internalname
        FROM 
            ""stats"".""competition"";";

        public static string GetById = @"
        SELECT
            id as competitionid,
            name,
            internal_name as internalname
        FROM
            ""stats"".""competition""
        WHERE
            id = @CompetitionId";
    }
}
