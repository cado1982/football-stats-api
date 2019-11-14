using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootballStatsApi.Controllers
{
    [AllowAnonymous]
    public class HealthCheckController : ControllerBase
    {
        [Route("healthcheck")]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
