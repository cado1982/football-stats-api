using System;
using Newtonsoft.Json;

namespace FootballStatsApi.Scraper.LeagueSummary
{
    public class Ppda
    {
        [JsonProperty("att")]
        public int PassesAllowed { get; set; }

        [JsonProperty("def")]
        public int DefensiveActions { get; set; }
    }
}

