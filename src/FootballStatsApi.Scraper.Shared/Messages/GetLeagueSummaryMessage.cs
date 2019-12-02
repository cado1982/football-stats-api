namespace FootballStatsApi.Scraper.Shared.Messages
{
    public class GetLeagueSummaryMessage : IAmqpMessage
    {
        public int CompetitionId { get; set; }
        public int? Season { get; set; }
    }
}