using System.Collections.Generic;
using Newtonsoft.Json;

namespace FootballStatsApi.Scraper.LeagueSummary.Models
{
    public class FixtureShots
    {
        [JsonProperty("h")]
        public List<FixtureShot> Home { get; set; }

        [JsonProperty("a")]
        public List<FixtureShot> Away { get; set; }
    }
}