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
    }
}
