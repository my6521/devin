using Devin.Reflection;
using Devin.Schedular;
using Devin.Schedular.Attributes;
using Devin.Schedular.Options;
using Devin.Utitlies;
using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TimeZoneConverter;

namespace Microsoft.AspNetCore.Builder
{
    public static class HangfireApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHangfireSetup(this IApplicationBuilder app, Action<IServiceProvider, HangfireConfig> setup = default)
        {
            var setting = app.ApplicationServices.GetService<HangfireConfig>();
            if (setting == null) throw new ArgumentNullException(nameof(setting));

            //启动Hangfire面板
            app.UseHangfireDashboard("/hf", new DashboardOptions
            {
                Authorization = new[]
                {
                    new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
                    {
                        RequireSsl = false,
                        SslRedirect = false,
                        LoginCaseSensitive = true,
                        Users = new[]
                        {
                            new BasicAuthAuthorizationUser
                            {
                                Login = setting.UserName,
                                PasswordClear = setting.Password
                            }
                        }
                    })
                },
            });

            setup?.Invoke(app.ApplicationServices, setting);

            return app;
        }

        /// <summary>
        /// 自动注入后台任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAutoInjectRecurringJob<T>(this IApplicationBuilder app) where T : IPrivateJob
        {
            var jobTypes = RuntimeUtil.AllTypes.Where(x => x.IsBasedOn<T>() && x.IsClass && !x.IsAbstract);
            foreach (var jobType in jobTypes)
            {
                var jobMeta = jobType.GetCustomAttribute<JobMetaAttribute>(false);
                if (jobMeta == null) continue;
                var jobId = jobMeta.JobId ?? jobType.Name;
                var interfaceType = jobType.GetInterfaces()[0];
                if (jobMeta.JobType == JobType.RecurringJob)
                {
                    var configureMethod = typeof(HangfireApplicationBuilderExtensions)
                    .GetMethod("AddOrUpdateRecurringJob", BindingFlags.Public | BindingFlags.Static)
                    ?.MakeGenericMethod(interfaceType)
                    ?.Invoke(null, new object[] { jobId, jobMeta.CronExpression });
                }
                else if (jobMeta.JobType == JobType.BackgroundJob)
                {
                    var configureMethod = typeof(HangfireApplicationBuilderExtensions)
                    .GetMethod("AddOrUpdateBackgroundJob", BindingFlags.Public | BindingFlags.Static)
                    ?.MakeGenericMethod(interfaceType)
                    ?.Invoke(null, new object[] { TimeSpan.FromSeconds(jobMeta.DelaySeconds) });
                }
            }

            return app;
        }

        /// <summary>
        /// 添加循环任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jobId"></param>
        /// <param name="cronExpression"></param>
        public static void AddOrUpdateRecurringJob<T>(string jobId, string cronExpression) where T : IPrivateJob
        {
            var tzi = TZConvert.GetTimeZoneInfo("Asia/Shanghai");
            RecurringJob.AddOrUpdate<T>(jobId, x => x.ExecuteAsync(), cronExpression, new RecurringJobOptions
            {
                TimeZone = tzi
            });
        }

        /// <summary>
        /// 添加单次任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="delay"></param>
        public static void AddOrUpdateBackgroundJob<T>(TimeSpan delay) where T : IPrivateJob
        {
            BackgroundJob.Schedule<T>(x => x.ExecuteAsync(), delay);
        }
    }
}