using System;

namespace FootballStatsApi.Domain.Entities
{
    public class PlayerSummary
    {
        public Competition Competition { get; set; }
        public int Season { get; set; }
        public Player Player { get; set; }
        public int Games { get; set; }
        public int MinutesPlayed { get; set; }
        public int Goals { get; set; }
        public float ExpectedGoals { get; set; }
        public int Assists { get; set; }
        public float ExpectedAssists { get; set; }
        public int Shots { get; set; }
        public int KeyPasses { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }
        public string Position { get; set; }
        public Team Team { get; set; }
        public int NonPenaltyGoals { get; set; }
        public float NonPenaltyExpectedGoals { get; set; }
        public float ExpectedGoalsChain { get; set; }
        public float ExpectedGoalsBuildup { get; set; }
    }
}
