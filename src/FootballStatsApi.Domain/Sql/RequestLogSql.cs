using System;
using System.Collections.Generic;
using System.Text;

namespace FootballStatsApi.Domain.Sql
{
    public static class RequestLogSql
    {
        public static string GetRateLimitInfo = @"
        SELECT
            COUNT(*) as requeststhisinterval
        FROM
            ""public"".""request_log"" rl
        WHERE
            rl.user_id = @UserId AND
            ""timestamp"" > DATE_TRUNC('hour', NOW());

        SELECT
            s.hourly_call_limit as intervalcalllimit,
            DATE_TRUNC('hour', NOW() + INTERVAL '1 hour') as nextintervalstart
        FROM
            ""identity"".""users"" u
        INNER JOIN
            ""public"".""subscriptions"" s ON s.id = u.subscription_id
        WHERE
            u.id = @UserId;";

        public static string Insert = @"
        INSERT INTO
            ""public"".""request_log"" (
                ""user_id"",
	            ""ip_address"",
	            ""response_ms"",
	            ""endpoint"",
                ""query_string"",
                ""http_method"",
                ""status_code""
            ) VALUES (
                @UserId,
                @IpAddress,
                @ResponseMs,
                @Endpoint,
                @QueryString,
                @HttpMethod,
                @StatusCode);";
    }
}
