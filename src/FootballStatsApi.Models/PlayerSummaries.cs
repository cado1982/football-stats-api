using System.Collections.Generic;

namespace FootballStatsApi.Models
{
    public class PlayerSummaries
    {
        public int Season { get; set; }
        public List<PlayerSummary> Players { get; set; }
    }
}