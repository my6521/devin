using Devin.Extensions.Hangfire.Internal;
using Hangfire;
using TimeZoneConverter;

namespace Devin.Extensions.Hangfire
{
    /// <summary>
    /// 作业扩展类
    /// </summary>
    public static class HangfireJobExtensions
    {
        /// <summary>
        /// 添加周期性任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queueName"></param>
        /// <param name="jobId"></param>
        /// <param name="cronExpression"></param>
        public static void AddOrUpdateRecurringJob<T>(string queueName, string jobId, string cronExpression) where T : IBaseJob
        {
            var tzi = TZConvert.GetTimeZoneInfo("Asia/Shanghai");
            RecurringJob.AddOrUpdate<T>(jobId, queueName, x => x.ExecuteAsync(), cronExpression, new RecurringJobOptions
            {
                TimeZone = tzi,
            });
        }

        /// <summary>
        /// 添加后台任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queueName"></param>
        /// <param name="delay"></param>
        public static void AddOrUpdateBackgroundJob<T>(string queueName, TimeSpan delay) where T : IBaseJob
        {
            BackgroundJob.Schedule<T>(queueName, x => x.ExecuteAsync(), delay);
        }
    }
}