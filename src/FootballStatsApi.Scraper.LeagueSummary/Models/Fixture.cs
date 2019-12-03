using System;
using Newtonsoft.Json;

namespace FootballStatsApi.Scraper.LeagueSummary.Models
{
    public class Fixture
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("isResult")]
        public bool IsResult { get; set; }

        [JsonProperty("h")]
        public FixtureTeam HomeTeam { get; set; }

        [JsonProperty("a")]
        public FixtureTeam AwayTeam { get; set; }

        // [JsonProperty("goals")]
        // public Goals<int> Goals { get; set; }

        // [JsonProperty("xG")]
        // public Goals<double> ExpectedGoals { get; set; }

        [JsonProperty("datetime")]
        public DateTimeOffset DateTime { get; set; }

        // [JsonProperty("forecast")]
        // public Forecast Forecast { get; set; }
    }
}