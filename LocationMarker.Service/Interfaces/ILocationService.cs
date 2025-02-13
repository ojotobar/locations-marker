using API.Common.Response.Model.Responses;
using LocationMarker.Shared.DTOs;

namespace LocationMarker.Service.Interfaces
{
    public interface ILocationService
    {
        Task<ApiBaseResponse> GetAllCountries(SearchDto searchDto);
        Task<ApiBaseResponse> GetCountry(Guid id);
        Task<ApiBaseResponse> GetCountryStates(Guid countryId);
        Task<ApiBaseResponse> GetState(Guid countryId, Guid id);
        Task<ApiBaseResponse> GetStateCities(Guid countryId, Guid stateId);
    }
}
