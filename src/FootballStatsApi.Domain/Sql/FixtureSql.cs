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
    }
}
