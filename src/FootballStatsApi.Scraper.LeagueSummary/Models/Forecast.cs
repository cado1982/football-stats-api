using Newtonsoft.Json;

namespace FootballStatsApi.Scraper.LeagueSummary.Models
{
    public class Forecast
    {
        [JsonProperty("w")]
        public double HomeWin { get; set; }

        [JsonProperty("d")]
        public double Draw { get; set; }

        [JsonProperty("l")]
        public double AwayWin { get; set; }
    }
}