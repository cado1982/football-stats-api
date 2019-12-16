using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace FootballStatsApi.Domain.Entities
{
    public class RequestLog
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public IPAddress IpAddress { get; set; }
        public int ResponseMs { get; set; }
        public string Endpoint { get; set; }
        public DateTime Timestamp { get; set; }
        public string HttpMethod { get; set; }
        public string QueryString { get; set; }
        public int StatusCode { get; set; }
    }
}
