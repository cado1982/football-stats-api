using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootballStatsApi.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HealthCheckController : ControllerBase
    {
        [Route("healthcheck")]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
