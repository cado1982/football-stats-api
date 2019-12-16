using System;

namespace FootballStatsApi.Domain.Entities
{
    public class RateLimitStatus
    {
        public long RequestsThisInterval { get; set; }
        public int IntervalCallLimit { get; set; }
        public DateTime NextIntervalStart { get; set; }
    }
}
