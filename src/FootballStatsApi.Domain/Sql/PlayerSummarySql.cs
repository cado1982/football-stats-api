using System;
using System.Collections.Generic;
using System.Text;

namespace FootballStatsApi.Domain.Sql
{
    public static class PlayerSummarySql
    {
        public static string Get = @"
        SELECT
            ps.games,
            ps.time as minutesplayed,
            ps.goals,
            ps.expected_goals as expectedgoals,
            ps.assists,
            ps.expected_assists as expectedassists,
            ps.shots,
            ps.key_passes as keypasses,
            ps.yellow_cards as yellowcards,
            ps.red_cards as redcards,
            ps.position,
            ps.team,
            ps.non_penalty_goals as nonpenaltygoals,
            ps.non_penalty_expected_goals as nonpenaltyexpectedgoals,
            ps.expected_goals_chain as expectedgoalschain,
            ps.expected_goals_buildup as expectedgoalsbuildup,
            p.id,
            p.name
        FROM 
            ""stats"".""player_summary"" ps
        INNER JOIN
            ""stats"".""player"" p ON p.id = ps.player_id 
        WHERE
            season_id = @SeasonId AND
            competition_id = @CompetitionId;";
    }
}
