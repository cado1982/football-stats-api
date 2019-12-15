using System;
using System.Collections.Generic;
using System.Text;

namespace FootballStatsApi.Domain.Sql
{
    public static class RateLimitSql
    {
        public static string Get = @"";

        public static string Insert = @"
        INSERT INTO
            stats.request_log (
                ""user_id"",
	            ""ip_address"",
	            ""response_ms"",
	            ""endpoint"",
                ""timestamp""
            ) VALUES (
                @UserId,
                @IpAddress,
                @ResponseMs,
                @Endpoint,
                @Timestamp);";
    }
}
