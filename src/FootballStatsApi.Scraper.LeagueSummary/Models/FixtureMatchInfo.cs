using System;
using Newtonsoft.Json;

namespace FootballStatsApi.Scraper.LeagueSummary.Models
{
    public class FixtureMatchInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("fid")]
        public int Fid { get; set; } // Don't know what this is

        [JsonProperty("h")]
        public int HomeTeamId { get; set; }

        [JsonProperty("a")]
        public int AwayTeamId { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("league_id")]
        public int CompetitionId { get; set; }

        [JsonProperty("season")]
        public int Season { get; set; }

        [JsonProperty("h_goals")]
        public int HomeGoals { get; set; }

        [JsonProperty("a_goals")]
        public int AwayGoals { get; set; }

        [JsonProperty("team_h")]
        public string HomeTeamName { get; set; }

        [JsonProperty("team_a")]
        public string AwayTeamName { get; set; }

        [JsonProperty("h_xg")]
        public double HomeExpectedGoals { get; set; }

        [JsonProperty("a_xg")]
        public double AwayExpectedGoals { get; set; }

        [JsonProperty("h_w")]
        public double HomeWinForecast { get; set; }

        [JsonProperty("h_d")]
        public double DrawForecast { get; set; }

        [JsonProperty("h_l")]
        public double AwayWinForecast { get; set; }

        [JsonProperty("league")]
        public string CompetitionName { get; set; }

        [JsonProperty("h_shot")]
        public int HomeShots { get; set; }

        [JsonProperty("a_shot")]
        public int AwayShots { get; set; }

        [JsonProperty("h_shotOnTarget")]
        public int HomeShotsOnTarget { get; set; }

        [JsonProperty("a_shotOnTarget")]
        public int AwayShotsOnTarget { get; set; }

        [JsonProperty("h_deep")]
        public int HomeDeepPasses { get; set; }

        [JsonProperty("a_deep")]
        public int AwayDeepPasses { get; set; }

        [JsonProperty("a_ppda")]
        public double AwayPpda { get; set; }

        [JsonProperty("h_ppda")]
        public double HomePpda { get; set; }
    }
}