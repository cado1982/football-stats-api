using System;
using System.Collections.Generic;

namespace FootballStatsApi.Domain.Entities
{
    public class FixtureDetails
    {
        public List<FixtureShot> Shots { get; set; }
        public List<FixturePlayer> Players { get; set; }
        public int FixtureId { get; set; }
        public bool IsResult { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public Competition Competition { get; set; }
        public int Season { get; set; }
        public int? HomeDeepPasses { get; set; }
        public int? AwayDeepPasses { get; set; }
        public double? HomePpda { get; set; }
        public double? AwayPpda { get; set; }
        public double? ForecastHomeWin { get; set; }
        public double? ForecastDraw { get; set; }
        public double? ForecastAwayWin { get; set; }
        public double? HomeExpectedGoals { get; set; }
        public double? AwayExpectedGoals { get; set; }
        public int? HomeGoals { get; set; }
        public int? AwayGoals { get; set; }
        public int HomeShots { get; set; }
        public int AwayShots { get; set; }
        public int HomeShotsOnTarget { get; set; }
        public int AwayShotsOnTarget { get; set; }
    }
}


