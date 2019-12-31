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

        public static string InsertMultiple = @"INSERT INTO ""stats"".""team"" (id, name, short_name) VALUES (@Id, @Name, @ShortName) ON CONFLICT(id) DO NOTHING;";
    }
}
