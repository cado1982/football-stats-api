using System.Collections.Generic;

namespace FootballStatsApi.Models
{
    public class TeamSummaries
    {
        public int Season { get; set; }
        public int CompetitionId { get; set; }
        public List<TeamSummary> Teams { get; set; }
    }
}