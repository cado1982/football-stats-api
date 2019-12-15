using System;
using System.Collections.Generic;
using System.Text;

namespace FootballStatsApi.Domain.Entities
{
    public class RequestLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string IpAddress { get; set; }
        public int ResponseMs { get; set; }
        public string Endpoint { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
