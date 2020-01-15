using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FootballStatsApi.Domain.Helpers;
using FootballStatsApi.Domain.Repositories;
using FootballStatsApi.Logic.v0.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FootballStatsApi.Handlers
{
    public class RateLimitMiddleware
    {
        private readonly RequestDelegate _next;

        public RateLimitMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context, ILogger<RateLimitMiddleware> logger, IConnectionProvider connectionProvider, IRequestLogRepository requestLogRepository)
        {
            logger.LogTrace("Entered InvokeAsync");

            var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);

            if (userIdClaim != null) 
            {
                var userId = int.Parse(userIdClaim.Value);

                using (var conn = await connectionProvider.GetOpenConnectionAsync())
                {
                    var rateLimitStatus = await requestLogRepository.GetRateLimitInfoByUser(userId, conn);

                    if (rateLimitStatus == null)
                    {
                        logger.LogError("Rate limit cannot be established for user {0}", userId);
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        return;
                    }

                    var remaining = Math.Max(0, (rateLimitStatus.IntervalCallLimit - rateLimitStatus.RequestsThisInterval));

                    context.Response.Headers.Add("X-RateLimit-Limit", rateLimitStatus.IntervalCallLimit.ToString());
                    context.Response.Headers.Add("X-RateLimit-Remaining", Math.Max(0, remaining - 1).ToString());
                    context.Response.Headers.Add("X-RateLimit-Reset", new DateTimeOffset(rateLimitStatus.NextIntervalStart).ToUnixTimeSeconds().ToString());

                    if (remaining <= 0)
                    {
                        context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                        context.Response.Headers.Add("Retry-After", ((int)(rateLimitStatus.NextIntervalStart - DateTime.UtcNow).TotalSeconds).ToString());
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}