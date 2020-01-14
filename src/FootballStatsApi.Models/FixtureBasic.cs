using System;
using System.Collections.Generic;

namespace FootballStatsApi.Models
{
    public class FixtureBasic
    {
        public int FixtureId { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public Competition Competition { get; set; }
        public int Season { get; set; }
        
        public HomeAway<int> Shots { get; set; }
        public HomeAway<int> ShotsOnTarget { get; set; }
        public HomeAway<int> Goals { get; set; }
        public List<Substitution> Substituions { get; set; }
        public List<FixturePlayerBasic> Players { get; set; }
    }

    public class FixtureAdvanced : FixtureBasic
    {
        public HomeAway<int> DeepPasses { get; set; }
        public HomeAway<double> Ppda { get; set; }
        public HomeAway<double> ExpectedPoints { get; set; }
        public HomeAway<double> ExpectedGoals { get; set; }
        public FixtureForecast Forecast { get; set; }
    }

    public class FixtureExpert : FixtureAdvanced
    {

    }
}
