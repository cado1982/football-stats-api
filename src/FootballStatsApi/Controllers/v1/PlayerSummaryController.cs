using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FootballStatsApi.Managers;
using FootballStatsApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FootballStatsApi.Controllers.v1
{
    [ApiController]
    [Route("api/v1/player-summary")]
    [Produces("application/json")]
    public class PlayerSummaryController : ControllerBase
    {
        private readonly ILogger<PlayerSummaryController> _logger;
        private readonly IPlayerSummaryManager _playerSummaryManager;

        public PlayerSummaryController(ILogger<PlayerSummaryController> logger, IPlayerSummaryManager playerSummaryManager)
        {
            _logger = logger;
            _playerSummaryManager = playerSummaryManager;
        }

        /// <summary>
        /// Gets a collection of player summaries for the specified season
        /// </summary>
        /// <param name="season">The season to retrieve expressed as the year the season started.</param>
        /// <remarks>
        /// Sample request:
        ///     GET /player-summary/2019
        /// </remarks>
        /// <returns>A collection of player summaries for the specified season</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("{season}")]
        public async Task<IActionResult> Get([Range(2014, 2050)] int season)
        { 
            try
            {
                var playerSummaries = await _playerSummaryManager.GetAsync(season);
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
