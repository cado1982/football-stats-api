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
    [Route("v1/fixtures")]
    [Produces("application/json")]
    public class FixtureController : ControllerBase
    {
        private readonly ILogger<FixtureController> _logger;
        private readonly IFixtureManager _fixtureManager;

        public FixtureController(ILogger<FixtureController> logger, IFixtureManager fixtureManager)
        {
            _logger = logger;
            _fixtureManager = fixtureManager;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetFixtures([FromQuery] int? competitionId, [FromQuery] int? season, [FromQuery] int? teamId)
        //{
        //    try
        //    {
        //        if (competitionId == null && season == null && teamId == null)
        //        {
        //            return BadRequest("At least one of competitionId, season, or teamId must be provided");
        //        }

        //        var fixtures = await _fixtureManager.GetAsync(fixture);

        //        return Ok(fixtures);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Unable to get fixtures");
        //        return StatusCode((int)HttpStatusCode.InternalServerError);
        //    }
        //}

        /// <summary>
        /// Gets the details of a fixture.
        /// </summary>
        /// <param name="fixture">The fixture id to retrieve.</param>
        /// <remarks>
        /// Sample request:
        ///     GET /fixtures/1
        /// </remarks>
        /// <returns>The details of the requested fixture.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{fixture}/details")]
        public async Task<IActionResult> GetFixtureDetails(int fixture)
        { 
            try
            {
                var fixtureDetails = await _fixtureManager.GetDetailsAsync(fixture);

                if (fixtureDetails == null)
                {
                    return NotFound();
                }

                return Ok(fixtureDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get fixture {fixture}");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
