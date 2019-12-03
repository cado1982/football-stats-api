namespace FootballStatsApi.Domain.Entities
{
    public class FixturePlayer
    {
        public Player Player { get; set; }
        public int Minutes { get; set; }
        public string Position { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }
        public Player ReplacedBy { get; set; }
        public Player Replaced { get; set; }
        public double ExpectedGoalsChain { get; set; }
        public double ExpectedGoalsBuildup { get; set; }
        public int PositionOrder { get; set; }
        public Team Team { get; set; }
        public int KeyPasses { get; set; }
        public int Assists { get; set; }
        public int Goals { get; set; }
        public int OwnGoals { get; set; }
        public int Shots { get; set; }
        public double ExpectedGoals { get; set; }
        public double ExpectedAssists { get; set; }
    }
}


