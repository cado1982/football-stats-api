using System;
using System.Collections.Generic;
using System.Text;

namespace FootballStatsApi.Domain.Sql
{
    public static class PlayerSummarySql
    {
        public static string Get = @"
        SELECT 
            id,
            name as playername,
            games,
            time as minutesplayed,
            goals,
            expected_goals as expectedgoals,
            assists ,
            expected_assists as expectedassists,
            shots,
            key_passes as keypasses,
            yellow_cards as yellowcards,
            red_cards as redcards,
            position,
            team,
            non_penalty_goals as nonpenaltygoals,
            non_penalty_expected_goals as nonpenaltyexpectedgoals,
            expected_goals_chain as expectedgoalschain,
            expected_goals_buildup as expectedgoalsbuildup
        FROM 
            ""stats"".""player-summary""";
    }
}
