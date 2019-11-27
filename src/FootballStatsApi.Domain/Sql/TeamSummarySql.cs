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

        public static string GetById = @"
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
            competition_id = @CompetitionId AND
            ts.team_id = @TeamId;";

        public static string InsertMultiple = @"
        INSERT INTO ""stats"".""team_summary"" (team_id, season_id, competition_id, games, won, drawn,
            lost, goals_for, goals_against, points, expected_goals, expected_goals_against,
            expected_points, ppda, deep_passes) VALUES (@TeamId, @Season, @CompetitionId, @Games,
            @Won, @Drawn, @Lost, @GoalsFor, @GoalsAgainst, @Points, @ExpectedGoals,
            @ExpectedGoalsAgainst, @ExpectedPoints, @Ppda, @DeepPasses)
        ON CONFLICT(team_id, season_id, competition_id) DO UPDATE SET games = EXCLUDED.games, won = EXCLUDED.won,
        drawn = EXCLUDED.drawn, lost = EXCLUDED.lost, goals_for = EXCLUDED.goals_for,
        goals_against = EXCLUDED.goals_against, points = EXCLUDED.points,
        expected_goals = EXCLUDED.expected_goals, expected_goals_against = EXCLUDED.expected_goals_against,
        expected_points = EXCLUDED.expected_points, ppda = EXCLUDED.ppda, deep_passes = EXCLUDED.deep_passes;";
    }
}