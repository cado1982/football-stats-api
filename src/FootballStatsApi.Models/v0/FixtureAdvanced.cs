using System;
using System.Collections.Generic;

namespace FootballStatsApi.Models.v0
{
    public class FixtureAdvanced
    {
        public int FixtureId { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public Competition Competition { get; set; }
        public int Season { get; set; }
        public int HomeShots { get; set; }
        public int AwayShots { get; set; }
        public int HomeShotsOnTarget { get; set; }
        public int AwayShotsOnTarget { get; set; }
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
        public List<Substitution> Substitutions { get; set; }
        public List<FixturePlayerBasic> HomePlayers { get; set; }
        public List<FixturePlayerBasic> AwayPlayers { get; set; }
        public FixtureForecast Forecast { get; set; }
    }
}
