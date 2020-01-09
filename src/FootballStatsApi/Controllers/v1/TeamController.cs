using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FootballStatsApi.Logic.Managers;
using FootballStatsApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FootballStatsApi.Controllers.v1
{
    [ApiController]
    [Route("v1/teams")]
    public class TeamController : ControllerBase
    {
        private readonly ILogger<TeamController> _logger;
        private readonly ITeamManager _teamManager;

        public TeamController(ILogger<TeamController> logger, ITeamManager teamManager)
        {
            _logger = logger;
            _teamManager = teamManager;
        }
        
        /// <summary>
        /// Gets a collection of teams.
        /// </summary>
        /// <param name="competition">The competition id to retrieve. Use the /competitions endpoint to retrieve a list of available competitions.</param>
        /// <param name="season">The season to retrieve. Seasons are 4 digit numbers that represent the year the season stated.</param>
        /// <returns>A collection of teams.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromQuery][Required] int competition, [FromQuery][Required][Range(2014, 2050)] int season)
        {
            try
            {
                var teams = await _teamManager.GetBasicStatsAsync(season, competition);
                
                return Ok(teams);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);
                return ValidationProblem(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get teams for season {0} and competition {1}", season, competition);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
