using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FootballStatsApi.Logic.v0.Managers;
using FootballStatsApi.Models.v0;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace FootballStatsApi.Controllers.v0
{
    [ApiController]
    [Route("v0/competitions")]
    [Produces("application/json")]
    public class CompetitionController : ControllerBase
    {
        private readonly ILogger<CompetitionController> _logger;
        private readonly ICompetitionManager _competitionManager;

        public CompetitionController(ILogger<CompetitionController> logger, ICompetitionManager competitionManager)
        {
            _logger = logger;
            _competitionManager = competitionManager;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Gets a list of available competitions",
            OperationId = "GetCompetitions"
        )]
        [SwaggerResponse(200, "A list of available competitions", typeof(List<Competition>))]
        [SwaggerResponse(401)]
        [SwaggerResponse(500)]
        public async Task<IActionResult> Get()
        { 
            try
            {
                var competitions = await _competitionManager.GetAsync();
                return Ok(competitions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get competitions");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
