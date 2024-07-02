using Devin.Extensions.Hangfire.Internal;
using Hangfire;
using System.Reflection;
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
        /// <param name="type"></param>
        /// <param name="jobId"></param>
        /// <param name="cronExpression"></param>
        public static void AddOrUpdateRecurringJob(this Type type, string jobId, string cronExpression)
        {
            typeof(HangfireJobExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(x => x.Name == "AddOrUpdateRecurringJob" && x.GetParameters().Length == 2)
                .FirstOrDefault()
                ?.MakeGenericMethod(type)
                ?.Invoke(null, new object[] { jobId, cronExpression });
        }

        /// <summary>
        /// 添加周期性任务
        /// </summary>
        /// <param name="type"></param>
        /// <param name="queue"></param>
        /// <param name="jobId"></param>
        /// <param name="cronExpression"></param>
        public static void AddOrUpdateRecurringJob(this Type type, string jobId, string queue, string cronExpression)
        {
            typeof(HangfireJobExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(x => x.Name == "AddOrUpdateRecurringJob" && x.GetParameters().Length == 3)
                .FirstOrDefault()
                ?.MakeGenericMethod(type)
                ?.Invoke(null, new object[] { jobId, queue, cronExpression });
        }

        /// <summary>
        /// 添加一次性任务
        /// </summary>
        /// <param name="type"></param>
        public static void AddOrUpdateBackgroundJob(this Type type)
        {
            typeof(HangfireJobExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
                       .Where(x => x.Name == "AddOrUpdateBackgroundJob" && x.GetParameters().Length == 1)
                       .FirstOrDefault()
                       ?.MakeGenericMethod(type)
                       ?.Invoke(null, new object[] { TimeSpan.FromSeconds(1) });
        }

        /// <summary>
        /// 添加一次性任务
        /// </summary>
        /// <param name="type"></param>
        /// <param name="queue"></param>
        public static void AddOrUpdateBackgroundJob(this Type type, string queue)
        {
            typeof(HangfireJobExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
                       .Where(x => x.Name == "AddOrUpdateBackgroundJob" && x.GetParameters().Length == 2)
                       .FirstOrDefault()
                       ?.MakeGenericMethod(type)
                       ?.Invoke(null, new object[] { queue, TimeSpan.FromSeconds(1) });
        }

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