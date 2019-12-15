using System;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using FootballStatsApi.Domain.Entities;
using FootballStatsApi.Domain.Helpers;
using FootballStatsApi.Domain.Repositories.Interface;
using FootballStatsApi.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FootballStatsApi.Handlers
{
    public class RequestLogMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLogMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context, ILogger<ApiKeyMiddleware> logger, IRateLimitRepository rateLimitRepository, IConnectionProvider connectionProvider)
        {
            logger.LogTrace("Entered InvokeAsync");

            if (!context.Request.Path.StartsWithSegments("/v1"))
            {
                await _next(context);
                return;
            }

            var sw = new Stopwatch();
            sw.Start();
            await _next(context);
            sw.Stop();

            var requestLog = new RequestLog();
            requestLog.Endpoint = context.Request.Path;
            requestLog.IpAddress = context.Connection.RemoteIpAddress.ToString();
            requestLog.ResponseMs = (int)sw.ElapsedMilliseconds;

            var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);

            if (userIdClaim == null) throw new SecurityException("User id not found");
            requestLog.UserId = int.Parse(userIdClaim.Value);

            using (var conn = await connectionProvider.GetOpenConnectionAsync())
            {
                await rateLimitRepository.InsertRequestLog(requestLog, conn);
            }
            
            await _next(context);
        }
    }
}