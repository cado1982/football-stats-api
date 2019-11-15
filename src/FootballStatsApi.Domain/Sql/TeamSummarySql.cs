using System;
using System.Collections.Generic;
using System.Text;

namespace FootballStatsApi.Domain.Sql
{
    public static class TeamSummarySql
    {
        public static string Get = @"
        SELECT 
            ts.games,
            ts.won,
            ts.drawn,
            ts.lost,
            ts.goals_for as goalsfor,
            ts.goals_against as goalsagainst,
            ts.points,
            ts.expected_goals as expectedgoals,
            ts.expected_goals_against as expectedgoalsagainst,
            ts.expected_points as expectedpoints,
            ts.ppda,
            ts.deep_passes,
            t.id,
            t.name
        FROM 
            ""stats"".""team_summary"" ts
        INNER JOIN
            ""stats"".""team"" t ON t.id = ts.team_id
        WHERE
            season_id = @SeasonId AND
            competition_id = @CompetitionId;";
    }
}