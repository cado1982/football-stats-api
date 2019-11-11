using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FootballStatsApi.Managers;
using FootballStatsApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FootballStatsApi.Controllers
{
    [ApiController]
    [Route("api/v1/player-summary")]
    public class PlayerSummaryV1Controller : ControllerBase
    {
        private readonly ILogger<PlayerSummaryV1Controller> _logger;
        private readonly IPlayerSummaryManager _playerSummaryManager;

        public PlayerSummaryV1Controller(ILogger<PlayerSummaryV1Controller> logger, IPlayerSummaryManager playerSummaryManager)
        {
            _logger = logger;
            _playerSummaryManager = playerSummaryManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var playerSummaries = await _playerSummaryManager.GetAsync();
                return Ok(playerSummaries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get player summaries");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
