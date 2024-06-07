using Hangfire;

namespace Devin.Schedular
{
    public interface IRecurringJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        Task ExecuteAsync();
    }
}