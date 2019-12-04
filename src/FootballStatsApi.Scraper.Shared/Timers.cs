namespace FootballStatsApi.Scraper.Shared
{
    public static class Timers
    {
        // How often should the process to update all the outstanding fixture details be run
        public static int FixtureDetailsIntervalSeconds = 60 * 5; // 5 minutes

        // How often should the process to update the league summaries be run
        public static int LeagueSummaryIntervalSeconds = 60 * 60; // 1 hour

        // When a fixture details request fails, how long until the message should be retried
        public static int FixtureDetailsRetryBackoffSeconds = 30; // 30 seconds

        // When a league summary request fails, how long until the message should be retried
        public static int LeagueSummaryRetryBackoffSeconds = 5 * 60; // 5 minutes
    }
}