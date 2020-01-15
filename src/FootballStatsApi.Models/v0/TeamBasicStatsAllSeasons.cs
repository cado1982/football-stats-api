using System.Collections.Generic;

namespace FootballStatsApi.Models.v0
{
    public class TeamBasicStatsAllSeasons
    {
        public Team Team { get; set; }
        public List<TeamSummaryBasic> Seasons { get; set; }
    }
}