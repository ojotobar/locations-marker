using LocationMarker.Data.Interfaces;
using Microsoft.Extensions.Logging;
using Mongo.Common.Settings;

namespace LocationMarker.Data.Implementations
{
    public class RepositoryManager(HttpClient httpClient, MongoDbSettings settings,
        ILogger<LocationFromClientRepository> clientLogger) : IRepositoryManager
    {
        private readonly Lazy<ILocationFromClientRepository> _locationFromClientRepository = new(() =>
            new LocationFromClientRepository(httpClient, clientLogger));
        private readonly Lazy<ICityRepository> _cityRepository = new(() =>
            new CityRepository(settings));
        private readonly Lazy<ICountryRepository> _countryRepository = new(() =>
            new CountryRepository(settings));
        private readonly Lazy<IStateRepository> _stateRepository = new(() =>
            new StateRepository(settings));
        private readonly Lazy<IApiKeyRepository> _apiKeyRepository = new(() =>
            new ApiKeyRepository(settings));

        public ILocationFromClientRepository ClientLocation => _locationFromClientRepository.Value;
        public ICountryRepository Country => _countryRepository.Value;
        public IStateRepository State => _stateRepository.Value;
        public ICityRepository City => _cityRepository.Value;
        public IApiKeyRepository ApiKey => _apiKeyRepository.Value;
    }
}
