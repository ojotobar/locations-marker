using API.Common.Response.Model.Responses;
using LocationMarker.Data.Interfaces;
using LocationMarker.Entities.Models;
using LocationMarker.Service.Interfaces;
using LocationMarker.Shared.DTOs;
using LocationMarker.Shared.Extensions;
namespace LocationMarker.Service.Implementations
{
    public class LocationService(IRepositoryManager repository) : ILocationService
    {
        private readonly IRepositoryManager _repository = repository;

        public async Task<ApiBaseResponse> GetAllCountries(SearchDto searchDto)
        {
            var countries = _repository.Country.FindAsQueryable(c => !c.IsDeprecated)
                .OrderBy(x => x.Name)
                .Search(searchDto.Search);

            var pagedList = await Task.Run(() => 
                PagedList<Country>.Paginate(countries, searchDto.Page, searchDto.PageSize));

            return new OkResponse<PaginatedList<Country>>(pagedList);
        }

        public async Task<ApiBaseResponse> GetCountry(Guid id)
        {
            var country = await _repository.Country.FindOneAsync(c => c.Id == id);
            if(country == null)
            {
                return new NotFoundResponse("No country found with the Id");
            }

            return new OkResponse<Country>(country);
        }

        public async Task<ApiBaseResponse> GetCountryStates(Guid countryId)
        {
            var states = await Task.Run(() => 
                _repository.State.FindAsQueryable(s => !s.IsDeprecated && s.CountryId.Equals(countryId))
                .OrderBy(s =>s.Name)
                .ToList());

            return new OkResponse<List<State>>(states);
        }

        public async Task<ApiBaseResponse> GetState(Guid countryId, Guid id)
        {
            var state = await _repository.State
                .FindOneAsync(s => s.CountryId.Equals(countryId) && s.Id.Equals(id));

            if (state == null)
            {
                return new NotFoundResponse("No state found with the Id");
            }

            return new OkResponse<State>(state);
        }

        public async Task<ApiBaseResponse> GetStateCities(Guid countryId, Guid stateId)
        {
            var cities = await Task.Run(() =>
                _repository.City.FindAsQueryable(c => !c.IsDeprecated && c.CountryId.Equals(countryId) && c.StateId.Equals(stateId))
                .OrderBy(s => s.Name)
                .ToList());

            return new OkResponse<List<City>>(cities);
        }
    }
}
