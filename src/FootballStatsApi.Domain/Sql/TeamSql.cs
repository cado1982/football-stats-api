using System;
using System.Collections.Generic;
using System.Text;

namespace FootballStatsApi.Domain.Sql
{
    public static class TeamSql
    {
        public static string GetAll = @"SELECT id, name, short_name as shortname FROM ""stats"".""team""";

        public static string GetByCompetitionAndSeason = @"
            WITH fixtures AS (
	            SELECT
		            fixture_id,
		            home_team_id,
		            away_team_id,
		            competition_id,
		            season_id
	            FROM
		            ""stats"".""fixture""
	            WHERE
                    competition_id = @CompetitionId AND
                    season_id = @SeasonId
            )

            SELECT
                id,
                name,
                short_name as shortname
            FROM 
	            ""stats"".""team""
            WHERE
                (SELECT COUNT(*) FROM fixtures WHERE id = home_team_id OR id = away_team_id) > 0";

        
        public static string GetBasicStatsByCompetitionAndSeason = @"WITH fixtures AS (
	        SELECT
		        f.fixture_id,
		        f.home_team_id,
		        f.away_team_id,
		        COUNT(*) FILTER (WHERE fs.result = 'Goal' AND fs.team_id = f.home_team_id OR fs.result = 'OwnGoal' AND fs.team_id = f.away_team_id) as homegoals,
		        COUNT(*) FILTER (WHERE fs.result = 'Goal' AND fs.team_id = f.away_team_id OR fs.result = 'OwnGoal' AND fs.team_id = f.home_team_id) as awaygoals
	        FROM
		        ""stats"".""fixture"" f
	        LEFT JOIN
		        ""stats"".""fixture_shot"" fs ON fs.fixture_id = f.fixture_id
            WHERE
                f.season_id = @SeasonId AND
                f.competition_id = @CompetitionId AND
                f.details_saved IS NOT NULL
            GROUP BY
                f.fixture_id,
                f.home_team_id,
                f.away_team_id
            ), home_stats AS(
                SELECT
                    t.id,
                    t.name,
                    t.short_name,
                    COUNT(*) as played,
                    COUNT(*) FILTER (WHERE homegoals > awaygoals) as wins,
		            COUNT(*) FILTER(WHERE homegoals = awaygoals) as draws,
		            COUNT(*) FILTER(WHERE homegoals < awaygoals) as losses,
		            SUM(homegoals) AS goals,
                    SUM(awaygoals) AS goals_against,
                    COUNT(*) FILTER(WHERE homegoals > awaygoals) * 3 + COUNT(*) FILTER(WHERE homegoals = awaygoals) as points
                FROM
                    fixtures f
                INNER JOIN
		            ""stats"".""team"" t ON t.id = f.home_team_id
                GROUP BY
                    t.id,
                    t.name,
                    t.short_name
            ), away_stats AS(
                SELECT
                    t.id,
                    t.name,
                    t.short_name,
                    COUNT(*) as played,
                    COUNT(*) FILTER (WHERE homegoals < awaygoals) as wins,
		            COUNT(*) FILTER(WHERE homegoals = awaygoals) as draws,
		            COUNT(*) FILTER(WHERE homegoals > awaygoals) as losses,
		            SUM(awaygoals) AS goals,
                    SUM(homegoals) AS goals_against,
                    COUNT(*) FILTER(WHERE homegoals < awaygoals) * 3 + COUNT(*) FILTER(WHERE homegoals = awaygoals) as points
                FROM
                fixtures f
            INNER JOIN
		        ""stats"".""team"" t ON t.id = f.away_team_id
            GROUP BY
                t.id,
                t.name,
                t.short_name
            )

            SELECT
	            ROW_NUMBER() OVER(ORDER BY SUM(points) desc, SUM(goals) - SUM(goals_against) desc, SUM(goals) desc) as position,
	            SUM(played) as played,
	            SUM(wins) as won,
	            SUM(draws) as drawn,
	            SUM(losses) as lost,
	            SUM(goals) - SUM(goals_against) as goaldifference,
	            SUM(goals) as goals,
	            SUM(goals_against) as goalsagainst,
	            SUM(points) as points,
                id,
                name,
                short_name as shortname
            FROM
            (SELECT
                *
            FROM
                home_stats
            UNION ALL
            SELECT 
	            *
            FROM
                away_stats) ss
            GROUP BY
                id,
                name,
                short_name
            ORDER BY position;";

        public static string InsertMultiple = @"INSERT INTO ""stats"".""team"" (id, name, short_name) VALUES (@Id, @Name, @ShortName) ON CONFLICT(id) DO NOTHING;";
    }
}
