using System;
using System.Collections.Generic;
using System.Text;

namespace FootballStatsApi.Domain.Sql
{
    public static class TeamSummarySql
    {
        public static string GetBySeasonAndCompetition = @"
        SELECT 
            ts.season_id as season,
            ts.games,
            ts.won,
            ts.drawn,
            ts.lost,
            ts.goals_for as goalsfor,
            ts.goals_against as goalsagainst,
            ts.points,
            ts.expected_goals as expectedgoals,
            ts.non_penalty_expected_goals as nonpenaltyexpectedgoals,
            ts.expected_goals_against as expectedgoalsagainst,
            ts.non_penalty_expected_goals_against as nonpenaltyexpectedgoalsagainst,
            ts.expected_points as expectedpoints,
            ts.ppda,
            ts.opposition_ppda as oppositionppda,
            ts.deep_passes as deeppasses,
            ts.opposition_deep_passes as oppositiondeeppasses,
            t.id,
            t.name
        FROM 
            ""stats"".""team_summary"" ts
        INNER JOIN
            ""stats"".""team"" t ON t.id = ts.team_id
        WHERE
            season_id = @SeasonId AND
            competition_id = @CompetitionId;";

        public static string GetByTeamIdAndSeason = @"
        SELECT 
            ts.season_id as season,
            ts.games,
            ts.won,
            ts.drawn,
            ts.lost,
            ts.goals_for as goalsfor,
            ts.goals_against as goalsagainst,
            ts.points,
            ts.expected_goals as expectedgoals,
            ts.non_penalty_expected_goals as nonpenaltyexpectedgoals,
            ts.expected_goals_against as expectedgoalsagainst,
            ts.non_penalty_expected_goals_against as nonpenaltyexpectedgoalsagainst,
            ts.expected_points as expectedpoints,
            ts.ppda,
            ts.opposition_ppda as oppositionppda,
            ts.deep_passes as deeppasses,
            ts.opposition_deep_passes as oppositiondeeppasses,
            t.id,
            t.name
        FROM 
            ""stats"".""team_summary"" ts
        INNER JOIN
            ""stats"".""team"" t ON t.id = ts.team_id
        WHERE
            season_id = @SeasonId AND
            ts.team_id = @TeamId;";

        public static string GetByTeamId = @"
        SELECT 
	        ts.season_id as season,
	        ts.games,
	        ts.won,
	        ts.drawn,
	        ts.lost,
	        ts.goals_for as goalsfor,
	        ts.goals_against as goalsagainst,
	        ts.points,
	        ts.expected_goals as expectedgoals,
	        ts.non_penalty_expected_goals as nonpenaltyexpectedgoals,
	        ts.expected_goals_against as expectedgoalsagainst,
	        ts.non_penalty_expected_goals_against as nonpenaltyexpectedgoalsagainst,
	        ts.expected_points as expectedpoints,
	        ts.ppda,
	        ts.opposition_ppda as oppositionppda,
	        ts.deep_passes as deeppasses,
	        ts.opposition_deep_passes as oppositiondeeppasses,
	        t.id,
	        t.name
        FROM 
	        stats.team_summary ts
        INNER JOIN
	        stats.team t ON t.id = ts.team_id
        WHERE
	        ts.team_id = @TeamId;";

        public static string InsertMultiple = @"
        INSERT INTO ""stats"".""team_summary"" (
            team_id,
            season_id,
            competition_id,
            games,
            won,
            drawn,
            lost,
            goals_for,
            goals_against,
            points,
            expected_goals,
            non_penalty_expected_goals,
            expected_goals_against,
            non_penalty_expected_goals_against,
            expected_points,
            ppda,
            opposition_ppda,
            deep_passes,
            opposition_deep_passes
        ) VALUES (
            @TeamId,
            @Season,
            @CompetitionId,
            @Games,
            @Won,
            @Drawn,
            @Lost,
            @GoalsFor,
            @GoalsAgainst,
            @Points,
            @ExpectedGoals,
            @NonPenaltyExpectedGoals,
            @ExpectedGoalsAgainst,
            @NonPenaltyExpectedGoalsAgainst,
            @ExpectedPoints,
            @Ppda,
            @OppositionPpda,
            @DeepPasses,
            @OppositionDeepPasses
        )
        ON CONFLICT(team_id, season_id, competition_id) DO UPDATE SET 
            games = EXCLUDED.games,
            won = EXCLUDED.won,
            drawn = EXCLUDED.drawn,
            lost = EXCLUDED.lost,
            goals_for = EXCLUDED.goals_for,
            goals_against = EXCLUDED.goals_against,
            points = EXCLUDED.points,
            expected_goals = EXCLUDED.expected_goals,
            non_penalty_expected_goals = EXCLUDED.non_penalty_expected_goals,
            expected_goals_against = EXCLUDED.expected_goals_against,
            non_penalty_expected_goals_against = EXCLUDED.non_penalty_expected_goals_against,
            expected_points = EXCLUDED.expected_points,
            ppda = EXCLUDED.ppda,
            opposition_ppda = EXCLUDED.opposition_ppda,
            deep_passes = EXCLUDED.deep_passes,
            opposition_deep_passes = EXCLUDED.opposition_deep_passes;";
    }
}