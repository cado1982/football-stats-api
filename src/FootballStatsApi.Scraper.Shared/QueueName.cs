namespace FootballStatsApi.Scraper.Shared
{
    public static class QueueName
    {
        public static string GetLeagueSummary = "getLeagueSummary";
        public static string GetLeagueSummaryRetry = "getLeagueSummary.retry";
        public static string GetFixtureDetails = "getFixtureDetails";
        public static string GetFixtureDetailsRetry = "getFixtureDetails.retry";
        public static string GetPlayerDetails = "getPlayerDetails";
        public static string GetPlayerDetailsRetry = "getPlayerDetails.retry";
    }
}