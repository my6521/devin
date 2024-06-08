using Hangfire;

namespace Devin.Schedular
{
    public interface IPrivateJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        Task ExecuteAsync();
    }
}