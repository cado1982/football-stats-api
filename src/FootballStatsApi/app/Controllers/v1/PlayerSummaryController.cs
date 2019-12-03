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
    [Route("v1/player-summaries")]
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
        /// Gets a collection of player summaries.
        /// </summary>
        /// <param name="competition">The competition id to retrieve. Use the /competitions endpoint to retrieve a list of available competitions.</param>
        /// <param name="season">The season to retrieve. Seasons are 4 digit numbers that represent the year the season stated.</param>
        /// <remarks>Player summaries contain aggregate stats for individual players.
        /// Sample request: GET /player-summaries?season=2019&amp;competition=1</remarks>
        /// <returns>A collection of player summaries.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromQuery][Required] int competition, [FromQuery][Required][Range(2014, 2050)] int season)
        {
            try
            {
                var summaries = await _playerSummaryManager.GetAsync(season, competition);
                return Ok(summaries);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);
                return ValidationProblem(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get player summaries");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Gets a summary for a single player.
        /// </summary>
        /// <param name="player">The player id to retrieve.</param>
        /// <param name="competition">The competition id to retrieve. Use the /competitions endpoint to retrieve a list of available competitions.</param>
        /// <param name="season">The season to retrieve. Seasons are 4 digit numbers that represent the year the season stated.</param>
        /// <remarks>Player summaries contain aggregate stats for individual players.
        /// Sample request: GET /player-summaries/123?season=2019&amp;competition=1</remarks>
        /// <returns>A summary for a single player.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("{player}")]
        public async Task<IActionResult> GetById([FromRoute][Required] int player, [FromQuery][Required] int competition, [FromQuery][Range(2014, 2050)][Required] int season)
        {
            try
            {
                var playerSummary = await _playerSummaryManager.GetByIdAsync(player, season, competition);
                return Ok(playerSummary);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);
                return ValidationProblem(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get player summaries");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
