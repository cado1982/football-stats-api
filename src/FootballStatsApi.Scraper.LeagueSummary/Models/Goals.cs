using Newtonsoft.Json;

namespace FootballStatsApi.Scraper.LeagueSummary.Models
{
    public class Goals<T> where T: struct
    {
        [JsonProperty("h")]
        public T? Home { get; set; }

        [JsonProperty("a")]
        public T? Away { get; set; }
    }
}