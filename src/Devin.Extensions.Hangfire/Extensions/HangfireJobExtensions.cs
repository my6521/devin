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
        /// <param name="jobId"></param>
        /// <param name="cronExpression"></param>
        public static void AddOrUpdateRecurringJob<T>(string jobId, string cronExpression) where T : IBaseJob
        {
            var tzi = TZConvert.GetTimeZoneInfo("Asia/Shanghai");
            RecurringJob.AddOrUpdate<T>(jobId, x => x.ExecuteAsync(), cronExpression, new RecurringJobOptions
            {
                TimeZone = tzi,
            });
        }

        /// <summary>
        /// 添加周期性任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jobId"></param>
        /// <param name="queue"></param>
        /// <param name="cronExpression"></param>
        public static void AddOrUpdateRecurringJob<T>(string jobId, string queue, string cronExpression) where T : IBaseJob
        {
            var tzi = TZConvert.GetTimeZoneInfo("Asia/Shanghai");
            RecurringJob.AddOrUpdate<T>(jobId, queue, x => x.ExecuteAsync(), cronExpression, new RecurringJobOptions
            {
                TimeZone = tzi,
            });
        }

        /// <summary>
        /// 添加后台任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="delay"></param>
        public static void AddOrUpdateBackgroundJob<T>(TimeSpan delay) where T : IBaseJob
        {
            BackgroundJob.Schedule<T>(x => x.ExecuteAsync(), delay);
        }

        /// <summary>
        /// 添加后台任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue"></param>
        /// <param name="delay"></param>
        public static void AddOrUpdateBackgroundJob<T>(string queue, TimeSpan delay) where T : IBaseJob
        {
            BackgroundJob.Schedule<T>(queue, x => x.ExecuteAsync(), delay);
        }
    }
}