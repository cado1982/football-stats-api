namespace FootballStatsApi.Models.v0
{
    public class TeamBasicStatsBySeason
    {
        public int Season { get; set; }
        public int Games { get; set; }
        public int Won { get; set; }
        public int Drawn { get; set; }
        public int Lost { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int Points { get; set; }
        public double ExpectedGoals { get; set; }
        public double NonPenaltyExpectedGoals { get; set; }
        public double ExpectedGoalsAgainst { get; set; }
        public double NonPenaltyExpectedGoalsAgainst { get; set; }
        public double ExpectedPoints { get; set; }
        public double Ppda { get; set; }
        public double OppositionPpda { get; set; }
        public int DeepPasses { get; set; }
        public int OppositionDeepPasses { get; set; }
    }
}