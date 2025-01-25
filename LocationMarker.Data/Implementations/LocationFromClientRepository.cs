using LocationMarker.Data.Interfaces;
using LocationMarker.Entities.ClientModels;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;
using System.Web.Http;

namespace LocationMarker.Data.Implementations
{
    public class LocationFromClientRepository(HttpClient httpClient, ILogger<LocationFromClientRepository> logger) 
        : ILocationFromClientRepository
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ILogger<LocationFromClientRepository> _logger = logger;

        public async Task<(List<CountryFromClient> Countries, bool Success, HttpStatusCode Code)> GetCountriesAsync()
        {
            var response = await _httpClient.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                var countries = await response.Content.ReadFromJsonAsync<List<CountryFromClient>>();
                return (countries ?? [], true, HttpStatusCode.OK);
            }
            else
            {
                var error = await response.Content.ReadAsAsync<HttpError>();
                _logger.LogError(error.Message, error);
                return ([], false, response.StatusCode);
            }
        }

        public async Task<(CountryFromClient? Country, bool Success, HttpStatusCode Code)> GetCountryAsync(string countryIso2)
        {
            var response = await _httpClient.GetAsync($"{countryIso2}");
            if (response.IsSuccessStatusCode)
            {
                var country = await response.Content.ReadFromJsonAsync<CountryFromClient>();
                return (country, true, HttpStatusCode.OK);
            }
            else
            {
                var error = await response.Content.ReadAsAsync<HttpError>();
                _logger.LogError(error.Message, error);
                return (null, false, response.StatusCode);
            }
        }

        public async Task<(List<StateFromClient> States, bool Success, HttpStatusCode Code)> GetStatesAsync(string countryIso2)
        {
            var response = await _httpClient.GetAsync($"{countryIso2}/states");
            if (response.IsSuccessStatusCode)
            {
                var states = await response.Content.ReadFromJsonAsync<List<StateFromClient>>();
                return (states ?? [], true, HttpStatusCode.OK);
            }
            else
            {
                var error = await response.Content.ReadAsAsync<HttpError>();
                _logger.LogError(error.Message, error);
                return ([], false, response.StatusCode);
            }
        }

        public async Task<(StateFromClient? State, bool Success, HttpStatusCode Code)> GetStateAsync(string countryIso2, string stateIso2)
        {
            var response = await _httpClient.GetAsync($"{countryIso2}/states/{stateIso2}");
            if (response.IsSuccessStatusCode)
            {
                var state = await response.Content.ReadFromJsonAsync<StateFromClient>();
                return (state, true, HttpStatusCode.OK);
            }
            else
            {
                var error = await response.Content.ReadAsAsync<HttpError>();
                _logger.LogError(error.Message, error);
                return (null, false, response.StatusCode);
            }
        }

        public async Task<(List<CityFromClient> Cities, bool Success, HttpStatusCode Code)> GetCitiesAsync(string countryIso2, string stateIso2)
        {
            var response = await _httpClient.GetAsync($"{countryIso2}/states/{stateIso2}/cities");
            if (response.IsSuccessStatusCode)
            {
                var cities = await response.Content.ReadFromJsonAsync<List<CityFromClient>>();
                return (cities ?? [], true, HttpStatusCode.OK);
            }
            else
            {
                var error = await response.Content.ReadAsAsync<HttpError>();
                _logger.LogError(error.Message, error);
                return ([], false, response.StatusCode);
            }
        }
    }
}
