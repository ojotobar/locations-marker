namespace LocationMarker.Data.Interfaces
{
    public interface IRepositoryManager
    {
        ILocationFromClientRepository ClientLocation { get; }
        ICountryRepository Country { get; }
        IStateRepository State { get; }
        ICityRepository City { get; }
        IApiKeyRepository ApiKey { get; }
    }
}
