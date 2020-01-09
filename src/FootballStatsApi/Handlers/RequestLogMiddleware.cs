using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FootballStatsApi.Domain.Entities;
using FootballStatsApi.Domain.Helpers;
using FootballStatsApi.Domain.Repositories;
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

        public async Task InvokeAsync(HttpContext context, ILogger<RequestLogMiddleware> logger, IRequestLogRepository requestLogRepository, IConnectionProvider connectionProvider)
        {
            logger.LogTrace("Entered InvokeAsync");

            var sw = new Stopwatch();
            sw.Start();
            await _next(context);
            sw.Stop();

            var requestLog = new RequestLog();
            requestLog.Endpoint = context.Request.PathBase + context.Request.Path;
            requestLog.IpAddress = context.Connection.RemoteIpAddress;
            requestLog.ResponseMs = (int)sw.ElapsedMilliseconds;
            requestLog.HttpMethod = context.Request.Method;
            requestLog.QueryString = context.Request.QueryString.Value;
            requestLog.StatusCode = context.Response.StatusCode;

            var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);

            if (userIdClaim == null) 
            {
                logger.LogError("UserId claim is missing");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            requestLog.UserId = int.Parse(userIdClaim.Value);

            using (var conn = await connectionProvider.GetOpenConnectionAsync())
            {
                await requestLogRepository.InsertRequestLog(requestLog, conn);
            }
        }
    }
}