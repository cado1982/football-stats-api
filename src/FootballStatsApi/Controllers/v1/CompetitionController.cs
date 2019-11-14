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
    [Route("api/v1/competitions")]
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

        /// <summary>
        /// Gets a list of available competitions
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET /competitions
        /// </remarks>
        /// <returns>A collection of competitions that can be queried in this api.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
