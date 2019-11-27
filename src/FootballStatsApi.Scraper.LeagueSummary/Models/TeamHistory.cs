using System;
using Newtonsoft.Json;

namespace FootballStatsApi.Scraper.LeagueSummary
{
    public class TeamHistory
    {
        [JsonProperty("h_a")]
        public string HomeOrAway { get; set; }

        [JsonProperty("xG")]
        public double ExpectedGoals { get; set; }

        [JsonProperty("xGA")]
        public double ExpectedGoalsAgainst { get; set; }

        [JsonProperty("npxG")]
        public double NonPenaltyExpectedGoals { get; set; }

        [JsonProperty("npxGA")]
        public double NonPenaltyExpectedGoalsAgainst { get; set; }

        [JsonProperty("ppda")]
        public Ppda PPDA { get; set; }

        [JsonProperty("ppda_allowed")]
        public Ppda PPDAAllowed { get; set; }

        [JsonProperty("deep")]
        public int Deep { get; set; }

        [JsonProperty("deep_allowed")]
        public int DeepAllowed { get; set; }

        [JsonProperty("scored")]
        public int Scored { get; set; }

        [JsonProperty("missed")]
        public int Missed { get; set; }

        [JsonProperty("xpts")]
        public double ExpectedPoints { get; set; }

        [JsonProperty("result")]
        public string Result { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("wins")]
        public int Wins { get; set; }

        [JsonProperty("draws")]
        public int Draws { get; set; }

        [JsonProperty("loses")]
        public int Loses { get; set; }

        [JsonProperty("pts")]
        public int Points { get; set; }

        [JsonProperty("npxGD")]
        public double NonPenaltyExpectedGoalDifference { get; set; }
    }

}

