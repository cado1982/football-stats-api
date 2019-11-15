using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FootballStatsApi.Managers;
using FootballStatsApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FootballStatsApi.Controllers.v1
{
    [ApiController]
    [Route("v1/competitions/{competitionId}/team-summaries")]
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
        public async Task<IActionResult> Get(int competitionId, [FromQuery][Range(2014, 2050)] int season)
        {
            try
            {
                var teamSummaries = await _teamSummaryManager.GetAsync(season, competitionId);
                return Ok(teamSummaries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get team summaries");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
