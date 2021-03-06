using System.Collections.Generic;
using Newtonsoft.Json;

namespace FootballStatsApi.Scraper.LeagueSummary.Models
{
    public class Team
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Name { get; set; }

        [JsonProperty("history")]
        public List<TeamHistory> History { get; set; }
    }
}

