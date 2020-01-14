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
using Swashbuckle.AspNetCore.Annotations;

namespace FootballStatsApi.Controllers.v1
{
    [ApiController]
    [Route("v1/teams")]
    public class TeamSummaryController : ControllerBase
    {
        private readonly ILogger<TeamSummaryController> _logger;
        private readonly ITeamSummaryManager _teamSummaryManager;

        public TeamSummaryController(ILogger<TeamSummaryController> logger, ITeamSummaryManager teamSummaryManager)
        {
            _logger = logger;
            _teamSummaryManager = teamSummaryManager;
        }
        
        [HttpGet]
        [SwaggerOperation(
            Summary = "Gets a collection of team stats for the given competition and season",
            OperationId = "GetTeamsStatsByCompetitionAndSeason"
        )]
        [SwaggerResponse(200, "A collection of team stats", typeof(TeamSummaries))]
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
            Summary = "Gets a specific team's stats for the given competition and season",
            OperationId = "GetTeamStatsByCompetitionAndSeason"
        )]
        [SwaggerResponse(200, "A team's stats", typeof(TeamSummary))]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [SwaggerResponse(500)]
        public async Task<IActionResult> GetById([FromRoute(Name = "team_id")][Required] int team, [FromQuery][Required] int competition, [FromQuery][Required][Range(2014, 2050)] int season)
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
