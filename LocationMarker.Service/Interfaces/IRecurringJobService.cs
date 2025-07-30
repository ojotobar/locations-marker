using Hangfire;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;

namespace LocationMarker.Service.Interfaces
{
    [Queue("recurring")]
    public interface IRecurringJobService
    {
        [RecurringJob("0 0 1 * *")]
        Task AddOrRotateApiKey(PerformContext context);
    }
}
