using API.Common.Response.Model.Responses;
using Hangfire;
using LocationMarker.Service.Implementations;
using LocationMarker.Service.Interfaces;
using LocationsMarker.API.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LocationsMarker.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiversion}/tool")]
    public class ToolController : ApiControllerBase
    {
        /// <summary>
        /// Seeds contries, their states and cities to the database
        /// </summary>
        /// <param name="countriesInitial">The initial of the countries you want to seed.
        /// Entering letter a for instance will only get countries whose names start with letter a</param>
        /// <returns></returns>
        ///<response code="200">OK</response>
        ///<response code="401">Unauthorized</response>
        ///<response code="500">Server error</response>
        [PermissionFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("run-dataload/{countriesInitial}")]
        public IActionResult RunLocationDataLoad([FromRoute] char countriesInitial)
        {
            var jobId = BackgroundJob
                .Enqueue<ToolServiceV1>(x => x.RunLocationDataLoad(countriesInitial, null!));
            

            return Ok(new OkResponse<string>($"Dataload started in the background with id: {jobId}. You can monitor the progress on the Hangfire dashboard."));
        }
    }
}
