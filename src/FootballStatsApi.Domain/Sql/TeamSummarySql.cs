using System;
using System.Collections.Generic;
using System.Text;

namespace FootballStatsApi.Domain.Sql
{
    public static class TeamSummarySql
    {
        public static string Get = @"
        SELECT 
            team_id as TeamId,
            name as teamname,
            games,
            won,
            drawn,
            lost,
            goals_for as goalsfor,
            goals_against as goalsagainst,
            points,
            expected_goals as expectedgoals,
            expected_goals_against as expectedgoalsagainst,
            expected_points as expectedpoints,
            ppda,
            deep_passes
        FROM 
            ""stats"".""team_summary""
        WHERE
            season_id = @SeasonId AND
            competition_id = @CompetitionId;";
    }
}