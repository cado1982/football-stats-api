using Newtonsoft.Json;

namespace FootballStatsApi.Scraper.LeagueSummary.Models
{
    public class FixtureRosterEntry
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("goals")]
        public int Goals { get; set; }

        [JsonProperty("own_goals")]
        public int OwnGoals { get; set; }

        [JsonProperty("shots")]
        public int Shots { get; set; }

        [JsonProperty("xG")]
        public double ExpectedGoals { get; set; }

        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("player_id")]
        public int PlayerId { get; set; }

        [JsonProperty("team_id")]
        public int TeamId { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }

        [JsonProperty("player")]
        public string PlayerName { get; set; }

        [JsonProperty("h_a")]
        public string HomeOrAway { get; set; }

        [JsonProperty("yellow_card")]
        public int YellowCard { get; set; }

        [JsonProperty("red_card")]
        public int RedCard { get; set; }

        [JsonProperty("roster_in")]
        public int RosterIn { get; set; }

        [JsonProperty("roster_out")]
        public int RosterOut { get; set; }

        [JsonProperty("key_passes")]
        public int KeyPasses { get; set; }

        [JsonProperty("assists")]
        public int Assists { get; set; }

        [JsonProperty("xA")]
        public double ExpectedAssists { get; set; }

        [JsonProperty("xGChain")]
        public double ExpectedGoalsChain { get; set; }

        [JsonProperty("xGBuildup")]
        public double ExpectedGoalsBuildup { get; set; }

        [JsonProperty("positionOrder")]
        public int PositionOrder { get; set; }
    }
}