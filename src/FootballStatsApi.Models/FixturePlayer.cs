namespace FootballStatsApi.Models
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
    }
}
