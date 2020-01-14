namespace FootballStatsApi.Domain.Sql
{
    public static class FixtureSql
    {
        public static string GetDetails = @"
        SELECT
            f.fixture_id as fixtureid,
            f.season_id as season,
            f.home_deep as homedeeppasses,
            f.away_deep as awaydeeppasses,
            f.home_ppda as homeppda,
            f.away_ppda as awayppda,
            f.home_win_forecast as forecasthomewin,
            f.home_draw_forecast as forecastdraw,
            f.home_loss_forecast as forecastawaywin,
            f.datetime as datetime,
            c.id,
            c.name,
            ht.id,
            ht.name,
            ht.short_name as shortname,
            at.id,
            at.name,
            at.short_name as shortname
        FROM
            ""stats"".""fixture"" f
        INNER JOIN
            ""stats"".""team"" ht ON ht.id = f.home_team_id
        INNER JOIN
            ""stats"".""team"" at ON at.id = f.away_team_id
        INNER JOIN
            ""stats"".""competition"" c ON c.id = f.competition_id
        WHERE
            f.fixture_id = @FixtureId;";

        public static string GetAllDetailsForCompetitionAndSeason = @"
        SELECT
	        f.fixture_id as fixtureid,
	        f.datetime,
	        f.season_id as season,
	        f.is_result as isresult,
	        f.home_goals as homegoals,
	        f.away_goals as awaygoals,
	        f.home_shots as homeshots,
	        f.away_shots as awayshots,
	        f.home_shots_on_target as homeshotsontarget,
	        f.away_shots_on_target as awayshotsontarget,
	        f.home_deep as homedeeppasses,
	        f.away_deep as awaydeeppasses,
	        f.home_ppda as homeppda,
	        f.away_ppda as awayppda,
	        f.home_expected_goals as homeexpectedgoals,
	        f.away_expected_goals as awayexpectedgoals,
	        f.home_win_forecast as homewinforecast,
	        f.home_draw_forecast as drawforecast,
	        f.home_loss_forecast as awaywinforecast,
	        f.home_expected_points as homeexpectedpoints,
	        f.away_expected_points as awayexpectedpoints,
	        c.id,
	        c.name,
	        ht.id,
	        ht.name,
	        at.id,
	        at.name
        FROM
	        stats.fixture f
        INNER JOIN
	        stats.competition c ON c.id = f.competition_id
        INNER JOIN
	        stats.team ht ON ht.id = f.home_team_id
        INNER JOIN
	        stats.team at ON at.id = f.away_team_id
        WHERE
	        f.season_id = @SeasonId AND
	        f.competition_id = @CompetitionId AND
	        f.details_saved IS NOT NULL
        ORDER BY
	        f.datetime desc;";

        public static string GetPlayerDetailsForCompetitionAndSeason = @"
        SELECT
	        f.fixture_id as fixtureid,
	        fp.time as minutes,
	        fp.position,
	        fp.yellow_cards as yellowcards,
	        fp.red_cards as redcards,
	        fp.expected_goals_chain as expectedgoalschain,
	        fp.expected_goals_buildup as expectedgoalsbuildup,
	        fp.expected_goals as expectedgoals,
	        fp.expected_assists as expectedassists,
	        fp.shots,
	        fp.shots_on_target as shotsontarget,
	        fp.key_passes as keypasses,
	        fp.assists,
	        fp.goals,
	        fp.own_goals as owngoals,
	        fp.position_order as positionorder,
	        p.id,
	        p.name,
	        t.id,
	        t.name,
	        r.id,
	        r.name,
	        rb.id,
	        rb.name
        FROM
	        stats.fixture_player fp
        INNER JOIN
	        stats.fixture f ON f.fixture_id = fp.fixture_id
        INNER JOIN
	        stats.player p ON p.id = fp.player_id 
        INNER JOIN
	        stats.team t ON t.id = fp.team_id
        LEFT JOIN
	        stats.player r ON r.id = fp.replaced_id
        LEFT JOIN
	        stats.player rb ON rb.id = fp.replaced_by_id
        WHERE
	        f.competition_id = @CompetitionId AND
	        f.season_id = @SeasonId AND
	        f.details_saved IS NOT NULL";

        public static string GetFixturePlayers = @"
        SELECT
            fp.time as minutes,
            fp.position,
            fp.yellow_cards as yellowcards,
            fp.red_cards as redcards,
            fp.expected_goals_chain as expectedgoalschain,
            fp.expected_goals_buildup as expectedgoalsbuildup,
            p.id,
            p.name,
            rb.id,
            rb.name,
            r.id,
            r.name,
            t.id,
            t.name,
            t.short_name as shortname
        FROM
            ""stats"".""fixture_player"" fp
        INNER JOIN
            ""stats"".""player"" p ON p.id = fp.player_id
        LEFT JOIN
            ""stats"".""player"" rb ON rb.id = fp.replaced_by_id
        LEFT JOIN
            ""stats"".""player"" r ON r.id = fp.replaced_id
        INNER JOIN
            ""stats"".""team"" t ON t.id = fs.team_id
        WHERE
            fp.fixture_id = @FixtureId;";
        
        public static string GetFixtureShots = @"
        SELECT
            fs.shot_id as shotid,
            fs.fixture_id as fixtureid,
            fs.minute,
            fs.result,
            fs.x,
            fs.y,
            fs.expected_goal as expectedgoal,
            fs.situation,
            fs.shot_type as type,
            fs.last_action as lastaction,
            p.id,
            p.name,
            ap.id,
            ap.name,
            t.id,
            t.name,
            t.short_name as shortname
        FROM
            ""stats"".""fixture_shot"" fs
        INNER JOIN
            ""stats"".""player"" p ON p.id = fs.player_id
        INNER JOIN
            ""stats"".""team"" t ON t.id = fs.team_id
        LEFT JOIN
            ""stats"".""player"" ap ON ap.id = fs.assisted_by
        WHERE
            fs.fixture_id = @FixtureId;";

        public static string IsFixtureSaved = @"SELECT is_result FROM ""stats"".""fixture"" WHERE fixture_id = @FixtureId";

        public static string InsertMultiple = @" 
            INSERT INTO ""stats"".""fixture"" (
                fixture_id,
                home_team_id,
                away_team_id,
                season_id,
                competition_id,
                is_result,
                datetime,
                details_saved
            ) VALUES (
                @FixtureId,
                @HomeTeamId,
                @AwayTeamId,
                @SeasonId,
                @CompetitionId,
                @IsResult,
                @DateTime,
                NULL
            )
            ON CONFLICT(fixture_id) DO UPDATE SET
                home_team_id = EXCLUDED.home_team_id,
                away_team_id = EXCLUDED.away_team_id,
                season_id = EXCLUDED.season_id,
                competition_id = EXCLUDED.competition_id,
                is_result = EXCLUDED.is_result,
                datetime = EXCLUDED.datetime;";

        public static string Update = @"
            UPDATE 
                ""stats"".""fixture"" 
            SET
                is_result = true,
                home_deep = @HomeDeep,
                away_deep = @AwayDeep,
                home_win_forecast = @HomeWinForecast,
                home_draw_forecast = @DrawForecast,
                home_loss_forecast = @AwayWinForecast,
                home_ppda = @HomePpda,
                away_ppda = @AwayPpda,
                home_goals = @HomeGoals,
                away_goals = @AwayGoals,
                home_shots = @HomeShots,
                away_shots = @AwayShots,
                home_shots_on_target = @HomeShotsOnTarget,
                away_shots_on_target = @AwayShotsOnTarget,
                home_expected_goals = @HomeExpectedGoals,
                away_expected_goals = @AwayExpectedGoals,
                home_expected_points = @HomeExpectedPoints,
                away_expected_points = @AwayExpectedPoints
            WHERE
                fixture_id = @FixtureId";
        
        public static string GetFixtureIdsToCheck = @"
            SELECT
                fixture_id
            FROM
                ""stats"".""fixture""
            WHERE
                datetime < NOW() - INTERVAL '100 minutes' AND
                details_saved IS NULL;";

        public static string UpdateDetailsSaved = @"
            UPDATE
                ""stats"".""fixture""
            SET
                details_saved = NOW()
            WHERE
                fixture_id = @FixtureId;";

        public static string InsertFixturePlayers = @"
            INSERT INTO ""stats"".""fixture_player"" 
            (
                player_id,
                fixture_id,
                team_id,
                time,
                position,
                yellow_cards,
                red_cards,
                replaced_by_id,
                replaced_id,
                expected_goals_chain,
                expected_goals_buildup,
                shots,
                shots_on_target,
                key_passes,
                assists,
                goals,
                own_goals,
                expected_goals,
                expected_assists,
                position_order
            ) VALUES (
                @PlayerId,
                @FixtureId,
                @TeamId,
                @Time,
                @Position,
                @YellowCards,
                @RedCards,
                @ReplacedById,
                @ReplacedId,
                @ExpectedGoalsChain,
                @ExpectedGoalsBuildup,
                @Shots,
                @ShotsOnTarget,
                @KeyPasses,
                @Assists,
                @Goals,
                @OwnGoals,
                @ExpectedGoals
                @ExpectedAssists,
                @PositionOrder
            )
            ON CONFLICT(player_id, fixture_id) DO NOTHING;";

        public static string InsertFixtureShots = @"
            INSERT INTO ""stats"".""fixture_shot"" (
                shot_id,
                player_id,
                fixture_id,
                team_id,
                minute,
                result,
                x,
                y,
                expected_goal,
                situation,
                shot_type,
                last_action,
                assisted_by
            ) VALUES (
                @ShotId,
                @PlayerId,
                @FixtureId,
                @TeamId,
                @Minute,
                @Result,
                @X,
                @Y,
                @ExpectedGoal,
                @Situation,
                @ShotType,
                @LastAction,
                @AssistedById
            )
            ON CONFLICT(shot_id) DO NOTHING;";
    }
}
