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
            f.expected_home_goals as homeexpectedgoals,
            f.expected_away_goals as awayexpectedgoals,
            f.home_goals as homegoals,
            f.away_goals as awaygoals,
            f.datetime as datetime,
            f.home_shots as homeshots,
            f.away_shots as awayshots,
            f.home_shots_on_target as homeshotsontarget,
            f.away_shots_on_target as awayshotsontarget,
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
                expected_home_goals = @ExpectedHomeGoals,
                expected_away_goals = @ExpectedAwayGoals,
                home_goals = @HomeGoals,
                away_goals = @AwayGoals,
                home_shots = @HomeShots,
                away_shots = @AwayShots,
                home_win_forecast = @HomeWinForecast,
                home_draw_forecast = @DrawForecast,
                home_loss_forecast = @AwayWinForecast,
                home_shots_on_target = @HomeShotsOnTarget,
                away_shots_on_target = @AwayShotsOnTarget,
                home_ppda = @HomePpda,
                away_ppda = @AwayPpda
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
