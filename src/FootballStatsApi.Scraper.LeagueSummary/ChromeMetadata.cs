using Newtonsoft.Json;

namespace FootballStatsApi.Scraper.LeagueSummary
{
    public class ChromeMetadata
    {
        [JsonProperty("webSocketDebuggerUrl")]
        public string WSEndpoint { get; set; }
    }
}
