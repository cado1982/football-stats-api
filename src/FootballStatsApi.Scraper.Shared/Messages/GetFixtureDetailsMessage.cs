namespace FootballStatsApi.Scraper.Shared.Messages
{
    public class GetFixtureDetailsMessage : IAmqpMessage
    {
        public int FixtureId { get; set; }
    }
}