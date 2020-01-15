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
            ps.non_penalty_goals as nonpenaltygoals,
            ps.non_penalty_expected_goals as nonpenaltyexpectedgoals,
            ps.expected_goals_chain as expectedgoalschain,
            ps.expected_goals_buildup as expectedgoalsbuildup,
            p.id,
            p.name,
            t.id,
            t.name
        FROM 
            ""stats"".""player_summary"" ps
        INNER JOIN
            ""stats"".""player"" p ON p.id = ps.player_id 
        INNER JOIN
            ""stats"".""team"" t ON t.id = ps.team_id
        WHERE
            season_id = @SeasonId AND
            competition_id = @CompetitionId;";

        public static string GetById = @"
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
            ps.non_penalty_goals as nonpenaltygoals,
            ps.non_penalty_expected_goals as nonpenaltyexpectedgoals,
            ps.expected_goals_chain as expectedgoalschain,
            ps.expected_goals_buildup as expectedgoalsbuildup,
            p.id,
            p.name,
            t.id,
            t.name
        FROM 
            ""stats"".""player_summary"" ps
        INNER JOIN
            ""stats"".""player"" p ON p.id = ps.player_id 
        INNER JOIN
            ""stats"".""team"" t ON t.id = ps.team_id
        WHERE
            season_id = @SeasonId AND
            ps.player_id = @PlayerId;";

        public static string InsertMultiple = @"
        INSERT INTO ""stats"".""player_summary"" (player_id, season_id, competition_id, team_id, games, time, goals,
            expected_goals, assists, expected_assists, shots, key_passes, yellow_cards,
            red_cards, position, non_penalty_goals, non_penalty_expected_goals,
            expected_goals_chain, expected_goals_buildup) VALUES (@PlayerId, @Season, @CompetitionId, @TeamId, @Games,
            @MinutesPlayed, @Goals, @ExpectedGoals, @Assists, @ExpectedAssists, @Shots, @KeyPasses,
            @YellowCards, @RedCards, @Position, @NonPenaltyGoals, @NonPenaltyExpectedGoals, @ExpectedGoalsChain,
            @ExpectedGoalsBuildUp)
        ON CONFLICT(player_id, season_id, competition_id) DO UPDATE SET team_id = EXCLUDED.team_id, games = EXCLUDED.games, time = EXCLUDED.time,
        goals = EXCLUDED.goals, expected_goals = EXCLUDED.expected_goals, assists = EXCLUDED.assists,
        expected_assists = EXCLUDED.expected_assists, shots = EXCLUDED.shots,
        key_passes = EXCLUDED.key_passes, yellow_cards = EXCLUDED.yellow_cards,
        red_cards = EXCLUDED.red_cards, position = EXCLUDED.position, non_penalty_goals = EXCLUDED.non_penalty_goals,
        non_penalty_expected_goals = EXCLUDED.non_penalty_expected_goals,
        expected_goals_chain = EXCLUDED.expected_goals_chain, expected_goals_buildup = EXCLUDED.expected_goals_buildup;";
    }
}
