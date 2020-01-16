using System;

namespace FootballStatsApi.Models.v0
{
    public class TeamSummaryBasic
    {
        public Team Team { get; set; }
        public int Season { get; set; }
        public int Games { get; set; }
        public int Won { get; set; }
        public int Drawn { get; set; }
        public int Lost { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int Points { get; set; }
    }
}
