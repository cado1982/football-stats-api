using System;
using System.Collections.Generic;
using System.Text;

namespace FootballStatsApi.Domain.Entities
{
    public class RateLimitStatus
    {
        public int RemainingCalls { get; set; }
        public int AllowedCalls { get; set; }
        public TimeSpan AllowedPeriod { get; set; }
        public DateTime ResetsAt { get; set; }
    }
}
