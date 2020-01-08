using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using FootballStatsApi.Logic.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FootballStatsApi.Handlers
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context, ILogger<ApiKeyMiddleware> logger, IUserManager userManager)
        {
            logger.LogTrace("Entered InvokeAsync");

            if (!context.Request.Path.StartsWithSegments("/v1"))
            {
                await _next(context);
                return;
            }

            var apiKeyHeader = context.Request.Headers["X-API-Key"].FirstOrDefault();

            if (apiKeyHeader == null || !Guid.TryParse(apiKeyHeader, out var apiKey)) {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var user = await userManager.GetUserByApiKeyAsync(apiKey);
            
            if (user == null) 
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims);
        
            var principal = new GenericPrincipal(identity, new string[0]);
            context.User = principal;

            await _next(context);
        }
    }
}