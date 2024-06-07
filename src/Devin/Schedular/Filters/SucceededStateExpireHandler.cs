using Hangfire.States;
using Hangfire.Storage;

namespace Devin.Schedular.Filters
{
    public class SucceededStateExpireHandler : IStateHandler
    {
        private readonly TimeSpan _jobExpirationTimeout;

        public SucceededStateExpireHandler(TimeSpan jobExpirationTimeout)
        {
            _jobExpirationTimeout = jobExpirationTimeout;
        }

        public string StateName => SucceededState.StateName;

        public void Apply(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            context.JobExpirationTimeout = _jobExpirationTimeout;
        }

        public void Unapply(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
        }
    }
}