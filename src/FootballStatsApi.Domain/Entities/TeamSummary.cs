namespace FootballStatsApi.Domain.Entities
{
    public class TeamSummary
    {
        public Team Team { get; set; }
        public int Season { get; set; }
        public Competition Competition { get; set; }
        public int Games { get; set; }
        public int Won { get; set; }
        public int Drawn { get; set; }
        public int Lost { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int Points { get; set; }
        public short ExpectedGoals { get; set; }
        public short ExpectedGoalsAgainst { get; set; }
        public short ExpectedPoints { get; set; }
        public short Ppda { get; set; }
        public int DeepPasses { get; set; }
    }
}