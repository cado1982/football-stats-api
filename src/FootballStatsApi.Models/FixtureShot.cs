namespace FootballStatsApi.Models
{
    public class FixtureShot
    {
        public int ShotId { get; set; }
        public Player Player { get; set; }
        public Player Assist { get; set; }
        public int Minute { get; set; }
        public string Result { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double ExpectedGoal { get; set; }
        public string HomeOrAway { get; set; }
        public string Situation { get; set; }
        public string Type { get; set; }
        public string LastAction { get; set; }
    }
}
