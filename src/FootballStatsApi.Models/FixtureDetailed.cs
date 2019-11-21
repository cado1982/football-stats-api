using System;
using System.Collections.Generic;

namespace FootballStatsApi.Models
{
    public class FixtureDetails
    {
        //public List<FixtureShot> Shots { get; set; }
        // public List<FixturePlayer> Players { get; set; }
        public int FixtureId { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public Competition Competition { get; set; }
        public int Season { get; set; }
        public int HomeDeepPasses { get; set; }
        public int AwayDeepPasses { get; set; }
        public double HomePpda { get; set; }
        public double AwayPpda { get; set; }
        public double ForecastHomeWin { get; set; }
        public double ForecastDraw { get; set; }
        public double ForecastAwayWin { get; set; }
    }
}
