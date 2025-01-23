using Hangfire.Server;

namespace LocationMarker.Service.Interfaces
{
    public interface IToolServiceV1
    {
        Task RunLocationDataLoad(char countriesInitial, PerformContext context);
    }
}
