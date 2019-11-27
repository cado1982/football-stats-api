using Newtonsoft.Json;

namespace FootballStatsApi.Scraper.LeagueSummary.Models
{
    public class FixtureTeam
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("short_title")]
        public string ShortTitle { get; set; }
    }
}