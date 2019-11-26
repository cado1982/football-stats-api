using System;
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


public class TeamHistory 
{
    [JsonProperty("h_a")]
    public string HomeOrAway { get; set; }

    [JsonProperty("xG")]
    public float ExpectedGoals { get; set; }

    [JsonProperty("xGA")]
    public float ExpectedGoalsAgainst { get; set; }

    [JsonProperty("npxG")]
    public float NonPenaltyExpectedGoals { get; set; }

    [JsonProperty("npxGA")]
    public float NonPenaltyExpectedGoalsAgainst { get; set; }

    [JsonProperty("ppda")]
    public float PPDA { get; set; }

    [JsonProperty("ppda_allowed")]
    public float PPDAAllowed { get; set; }

    [JsonProperty("deep")]
    public int Deep { get; set; }

    [JsonProperty("deep_allowed")]
    public int DeepAllowed { get; set; }

    [JsonProperty("scored")]
    public int Scored { get; set; }

    [JsonProperty("missed")]
    public int Missed { get; set; }

    [JsonProperty("xpts")]
    public float ExpectedPoints { get; set; }

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
    public float NonPenaltyExpectedGoalDifference { get; set; }
}

