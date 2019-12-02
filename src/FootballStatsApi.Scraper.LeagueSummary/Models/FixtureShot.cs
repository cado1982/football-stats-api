using System;
using Newtonsoft.Json;

namespace FootballStatsApi.Scraper.LeagueSummary.Models
{
    public class FixtureShot
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("minute")]
        public int Minute { get; set; }

        [JsonProperty("result")]
        public string Result { get; set; }

        [JsonProperty("X")]
        public double X { get; set; }

        [JsonProperty("Y")]
        public double Y { get; set; }

        [JsonProperty("xG")]
        public double ExpectedGoal { get; set; }

        [JsonProperty("player")]
        public string PlayerName { get; set; }

        [JsonProperty("h_a")]
        public string HomeOrAway { get; set; }

        [JsonProperty("player_id")]
        public int PlayerId { get; set; }

        [JsonProperty("situation")]
        public string Situation { get; set; }

        [JsonProperty("season")]
        public int Season { get; set; }

        [JsonProperty("shotType")]
        public string ShotType { get; set; }

        [JsonProperty("match_id")]
        public int MatchId { get; set; }

        [JsonProperty("h_team")]
        public string HomeTeamName { get; set; }

        [JsonProperty("a_team")]
        public string AwayTeamName { get; set; }

        [JsonProperty("h_goals")]
        public int HomeGoals { get; set; }

        [JsonProperty("a_goals")]
        public int AwayGoals { get; set; }

        [JsonProperty("date")]
        public DateTimeOffset Date { get; set; }

        [JsonProperty("player_assisted")]
        public string PlayerAssistedName { get; set; }

        [JsonProperty("lastAction")]
        public string LastAction { get; set; }

    }
}