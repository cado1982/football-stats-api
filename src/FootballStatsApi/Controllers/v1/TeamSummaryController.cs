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
    [Route("v1/team-summaries")]
    public class TeamSummaryController : ControllerBase
    {
        private readonly ILogger<TeamSummaryController> _logger;
        private readonly ITeamSummaryManager _teamSummaryManager;

        public TeamSummaryController(ILogger<TeamSummaryController> logger, ITeamSummaryManager teamSummaryManager)
        {
            _logger = logger;
            _teamSummaryManager = teamSummaryManager;
        }
        
        /// <summary>
        /// Gets a collection of team summaries.
        /// </summary>
        /// <param name="competition">The competition id to retrieve. Use the /competitions endpoint to retrieve a list of available competitions.</param>
        /// <param name="season">The season to retrieve. Seasons are 4 digit numbers that represent the year the season stated.</param>
        /// <remarks>Team summaries contain aggregate stats for a team.
        /// Sample request: GET /team-summaries?season=2019&amp;competition=1</remarks>
        /// <returns>A collection of team summaries for the given season and competition.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromQuery][Required] int competition, [FromQuery][Required][Range(2014, 2050)] int season)
        {
            try
            {
                var teamSummaries = await _teamSummaryManager.GetAsync(season, competition);
                return Ok(teamSummaries);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);
                return ValidationProblem(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get team summaries for season {season} and competition {competition}");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Gets a summary for a single team.
        /// </summary>
        /// <param name="team">The team id to retrieve.</param>
        /// <param name="competition">The competition id to retrieve. Use the /competitions endpoint to retrieve a list of available competitions.</param>
        /// <param name="season">The season to retrieve. Seasons are 4 digit numbers that represent the year the season stated.</param>
        /// <remarks>Team summaries contain aggregate stats for individual teams.
        /// Sample request: GET /team-summaries/123?season=2019&amp;competition=1</remarks>
        /// <returns>A summary for a single team for the given competition and season.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("{team}")]
        public async Task<IActionResult> GetById([FromRoute][Required] int team, [FromQuery][Required] int competition, [FromQuery][Required][Range(2014, 2050)] int season)
        {
            try
            {
                var teamSummary = await _teamSummaryManager.GetByIdAsync(team, season, competition);
                return Ok(teamSummary);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);
                return ValidationProblem(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get team summary for team {team} and season {season} and competition {competition}");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
