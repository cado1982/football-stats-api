using System.Collections.Generic;
using Newtonsoft.Json;

namespace FootballStatsApi.Scraper.LeagueSummary.Models
{
    public class FixtureRosters
    {
        [JsonProperty("h")]
        public Dictionary<int, FixtureRosterEntry> Home { get; set; }

        [JsonProperty("a")]
        public Dictionary<int, FixtureRosterEntry> Away { get; set; }
    }
}