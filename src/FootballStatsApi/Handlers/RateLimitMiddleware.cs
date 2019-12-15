using System;
using System.Linq;
using System.Threading.Tasks;
using FootballStatsApi.Managers;
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

        public async Task InvokeAsync(HttpContext context, ILogger<RateLimitMiddleware> logger, IUserManager userManager)
        {
            logger.LogTrace("Entered InvokeAsync");

            await _next(context);
        }
    }
}