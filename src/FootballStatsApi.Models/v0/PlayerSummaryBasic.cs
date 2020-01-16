using System;

namespace FootballStatsApi.Models.v0
{
    public class PlayerSummaryBasic
    {
        public Player Player { get; set; }
        public int Games { get; set; }
        public int MinutesPlayed { get; set; }
        public int Goals { get; set; }
        public int NonPenaltyGoals { get; set; }
        public int Assists { get; set; }
        public int Shots { get; set; }
        public int KeyPasses { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }
        public string Position { get; set; }
        public Team Team { get; set; }
    }
}
