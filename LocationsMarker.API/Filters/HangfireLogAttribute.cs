using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Logging;
using Hangfire.States;
using Newtonsoft.Json;

namespace LocationsMarker.API.Filters
{
    public class HangfireLogAttribute : JobFilterAttribute, IElectStateFilter
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        // IElectState
        public void OnStateElection(ElectStateContext context)
        {
            var failedState = context.CandidateState as FailedState;
            if (failedState != null)
            {
                var json = JsonConvert.SerializeObject(failedState.Exception, Formatting.Indented);
                Logger.ErrorFormat($"Job {context.BackgroundJob.Id} failed due to an exception", json);
            }
        }
    }
}
