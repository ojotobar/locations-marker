﻿using API.Common.Response.Model.Responses;
using LocationMarker.Entities.ClientModels;
using LocationMarker.Entities.Models;
using LocationMarker.Service.Interfaces;
using LocationMarker.Shared.DTOs;
using LocationsMarker.API.Controllers.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace LocationsMarker.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiversion}/location")]
    public class LocationController(IServiceManager service) : ApiControllerBase
    {
        private readonly IServiceManager _service = service;

        /// <summary>
        /// Gets paged list of all countries
        /// </summary>
        /// <returns></returns>
        ///<response code="200">OK</response>
        ///<response code="500">Server error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("countries")]
        public async Task<IActionResult> GetCountries([FromQuery] SearchDto searchDto)
        {
            var baseResult = await _service.Location.GetAllCountries(searchDto);
            if (!baseResult.Success)
                return ProcessError(baseResult);

            return Ok(baseResult.GetResult<PaginatedList<Country>>());
        }

        /// <summary>
        /// Gets paged list of all countries
        /// </summary>
        /// <returns></returns>
        ///<response code="200">OK</response>
        ///<response code="500">Server error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("country/{countryId}/states")]
        public async Task<IActionResult> GetCountryStates([FromRoute] Guid countryId)
        {
            var baseResult = await _service.Location.GetCountryStates(countryId);
            if (!baseResult.Success)
                return ProcessError(baseResult);

            return Ok(baseResult.GetResult<List<State>>());
        }

        /// <summary>
        /// Gets paged list of all countries
        /// </summary>
        /// <returns></returns>
        ///<response code="200">OK</response>
        ///<response code="500">Server error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("country/{countryId}/state/{stateId}/cities")]
        public async Task<IActionResult> GetStateCities([FromRoute] Guid countryId, [FromRoute] Guid stateId)
        {
            var baseResult = await _service.Location.GetStateCities(countryId, stateId);
            if (!baseResult.Success)
                return ProcessError(baseResult);

            return Ok(baseResult.GetResult<List<City>>());
        }

        /// <summary>
        /// Gets a country by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ///<response code="200">OK</response>
        ///<response code="404">Not Found</response>
        ///<response code="500">Server error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType (StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("country/{id}")]
        public async Task<IActionResult> GetCountry([FromRoute] Guid id)
        {
            var baseResult = await _service.Location.GetCountry(id);
            if (!baseResult.Success)
                return ProcessError(baseResult);

            return Ok(baseResult.GetResult<Country>());
        }

        /// <summary>
        /// Gets a State by Id
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        ///<response code="200">OK</response>
        ///<response code="404">Not Found</response>
        ///<response code="500">Server error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("country/{countryId}/state/{id}")]
        public async Task<IActionResult> GetState([FromRoute] Guid countryId, [FromRoute] Guid id)
        {
            var baseResult = await _service.Location.GetState(countryId, id);
            if (!baseResult.Success)
                return ProcessError(baseResult);

            return Ok(baseResult.GetResult<State>());
        }
    }
}