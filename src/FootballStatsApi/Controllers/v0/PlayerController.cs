using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FootballStatsApi.Logic.v0.Managers;
using FootballStatsApi.Models.v0;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace FootballStatsApi.Controllers.v0
{
    [ApiController]
    [Route("v0/players")]
    [Produces("application/json")]
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerController> _logger;
        private readonly IPlayerSummaryManager _playerSummaryManager;

        public PlayerController(ILogger<PlayerController> logger, IPlayerSummaryManager playerSummaryManager)
        {
            _logger = logger;
            _playerSummaryManager = playerSummaryManager;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Gets a collection of player stats for the given competition and season",
            OperationId = "GetPlayersStatsByCompetitionAndSeason"
        )]
        [SwaggerResponse(200, "A collection of player stats", typeof(List<PlayerSummaryBasic>))]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [SwaggerResponse(500)]
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

        [HttpGet]
        [SwaggerOperation(
            Summary = "Gets a specific player's stats for the given season",
            OperationId = "GetPlayerStatsBySeason"
        )]
        [SwaggerResponse(200, "A player's stats", typeof(PlayerSummaryBasic))]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [SwaggerResponse(500)]
        [Route("{player}/{season}")]
        public async Task<IActionResult> GetById([FromRoute][Required] int player, [FromRoute][Range(2014, 2050)][Required] int season)
        {
            try
            {
                var playerSummary = await _playerSummaryManager.GetByIdAsync(player, season);
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
