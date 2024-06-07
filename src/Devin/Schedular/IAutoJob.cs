using Hangfire;

namespace Devin.Schedular
{
    public interface IAutoJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        Task Execute();
    }
}