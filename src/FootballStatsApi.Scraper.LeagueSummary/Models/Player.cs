using Newtonsoft.Json;

namespace FootballStatsApi.Scraper.LeagueSummary.Models
{
    public class Player
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("player_name")]
        public string PlayerName { get; set; }

        [JsonProperty("games")]
        public int Games { get; set; }

        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("goals")]
        public int Goals { get; set; }

        [JsonProperty("xG")]
        public double ExpectedGoals { get; set; }

        [JsonProperty("assists")]
        public int Assists { get; set; }

        [JsonProperty("xA")]
        public double ExpectedAssists { get; set; }

        [JsonProperty("shots")]
        public int Shots { get; set; }

        [JsonProperty("key_passes")]
        public int KeyPasses { get; set; }

        [JsonProperty("yellow_cards")]
        public int YellowCards { get; set; }

        [JsonProperty("red_cards")]
        public int RedCards { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }

        [JsonProperty("team_title")]
        public string TeamTitle { get; set; }

        [JsonProperty("npg")]
        public int NonPenaltyGoals { get; set; }

        [JsonProperty("npxG")]
        public double NonPenaltyExpectedGoals { get; set; }

        [JsonProperty("xGChain")]
        public double ExpectedGoalsChain { get; set; }

        [JsonProperty("xGBuildup")]
        public double ExpectedGoalsBuildup { get; set; }
    }
}

