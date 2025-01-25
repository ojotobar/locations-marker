using LocationMarker.Data.Implementations;
using LocationMarker.Data.Interfaces;
using LocationMarker.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LocationMarker.Service.Implementations
{
    public class ServiceManager(IRepositoryManager repository, 
        ILogger<ToolServiceV1> toolLogger) : IServiceManager
    {
        private readonly Lazy<ILocationService> _locationService = new(() =>
            new LocationService(repository));
        private readonly Lazy<IToolServiceV1> _toolServiceV1 = new(() =>
            new ToolServiceV1(repository, toolLogger));

        public ILocationService Location => _locationService.Value;

        public IToolServiceV1 ToolV1 => _toolServiceV1.Value;
    }
}
