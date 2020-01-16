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
    [Route("v0/teams")]
    [Produces("application/json")]
    public class TeamController : ControllerBase
    {
        private readonly ILogger<TeamController> _logger;
        private readonly ITeamSummaryManager _teamSummaryManager;

        public TeamController(ILogger<TeamController> logger, ITeamSummaryManager teamSummaryManager)
        {
            _logger = logger;
            _teamSummaryManager = teamSummaryManager;
        }
        
        [HttpGet]
        [SwaggerOperation(
            Summary = "Gets a collection of team stats for the given competition and season",
            OperationId = "GetTeamsStatsByCompetitionAndSeason"
        )]
        [SwaggerResponse(200, "A collection of team stats", typeof(List<TeamSummaryBasic>))]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [SwaggerResponse(500)]
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

        [HttpGet]
        [Route("{team_id}")]
        [SwaggerOperation(
            Summary = "Gets a specific team's stats for all seasons",
            OperationId = "GetTeamStats"
        )]
        [SwaggerResponse(200, "A team's stats", typeof(List<TeamSummaryBasic>))]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [SwaggerResponse(500)]
        public async Task<IActionResult> GetById([FromRoute(Name = "team_id")][Required] int teamId)
        {
            try
            {
                var teamSummaries = await _teamSummaryManager.GetByIdAsync(teamId);
                return Ok(teamSummaries);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);
                return ValidationProblem(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get team stats for team {0}", teamId);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("{team_id}/{season}")]
        [SwaggerOperation(
            Summary = "Gets a specific team's stats for one season",
            OperationId = "GetTeamStatsBySeason"
        )]
        [SwaggerResponse(200, "A team's stats", typeof(TeamSummaryBasic))]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [SwaggerResponse(404, "Team not found")]
        [SwaggerResponse(500)]
        public async Task<IActionResult> GetByIdAndSeason([FromRoute(Name = "team_id")][Required] int teamId, [FromRoute][Required][Range(2014, 2050)] int season)
        {
            try
            {
                var teamSummary = await _teamSummaryManager.GetByIdAsync(teamId, season);

                if (teamSummary == null) return NotFound("Team or season not found");
                return Ok(teamSummary);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);
                return ValidationProblem(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get team stats for team {0} and season {1}", teamId, season);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
