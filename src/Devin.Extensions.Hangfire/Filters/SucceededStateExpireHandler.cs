using Hangfire.States;
using Hangfire.Storage;

namespace Devin.Extensions.Hangfire.Filters
{
    /// <summary>
    /// 设置超时时间
    /// </summary>
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