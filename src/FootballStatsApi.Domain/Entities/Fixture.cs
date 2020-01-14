using System;
using System.Collections.Generic;

namespace FootballStatsApi.Domain.Entities
{
    public class Fixture
    {
        public int FixtureId { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public int Season { get; set; }
        public bool IsResult { get; set; }
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
        public int HomeShots { get; set; }
        public int AwayShots { get; set; }
        public int HomeShotsOnTarget { get; set; }
        public int AwayShotsOnTarget { get; set; }
        public int HomeDeep { get; set; }
        public int AwayDeep { get; set; }
        public double HomePpda { get; set; }
        public double AwayPpda { get; set; }
        public double HomeExpectedGoals { get; set; }
        public double AwayExpectedGoals { get; set; }
        public double HomeWinForecast { get; set; }
        public double DrawForecast { get; set; }
        public double AwayWinForecast { get; set; }
        public double HomeExpectedPoints { get; set; }
        public double AwayExpectedPoints { get; set; }
        public Competition Competition { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
    }
}


