using LocationMarker.Entities.ClientModels;
using System.Net;

namespace LocationMarker.Data.Interfaces
{
    public interface ILocationFromClientRepository
    {
        Task<(List<CityFromClient> Cities, bool Success, HttpStatusCode Code)> GetCitiesAsync(string countryIso2, string stateIso2);
        Task<(List<CountryFromClient> Countries, bool Success, HttpStatusCode Code)> GetCountriesAsync();
        Task<(CountryFromClient? Country, bool Success, HttpStatusCode Code)> GetCountryAsync(string countryIso2);
        Task<(StateFromClient? State, bool Success, HttpStatusCode Code)> GetStateAsync(string countryIso2, string stateIso2);
        Task<(List<StateFromClient> States, bool Success, HttpStatusCode Code)> GetStatesAsync(string countryIso2);
    }
}
