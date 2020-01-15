using System;
using System.Collections.Generic;
using System.Text;

namespace FootballStatsApi.Models.v0
{
    public class TeamBasicStats
    {
        public Team Team { get; set; }
        public int Position { get; set; }
        public int Played { get; set; }
        public int Won { get; set; }
        public int Drawn { get; set; }
        public int Lost { get; set; }
        public int GoalDifference { get; set; }
        public int Goals { get; set; }
        public int GoalsAgainst { get; set; }
        public int Points { get; set; }
    }
}
