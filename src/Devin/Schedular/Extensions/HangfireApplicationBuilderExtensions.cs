using Devin.Reflection;
using Devin.Schedular;
using Devin.Schedular.Attributes;
using Devin.Schedular.Filters;
using Devin.Schedular.Options;
using Devin.Utitlies;
using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

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
            if (setting.JobExpirationTimeout > 0)
            {
                GlobalStateHandlers.Handlers.Add(new SucceededStateExpireHandler(TimeSpan.FromMinutes(setting.JobExpirationTimeout)));
            }

            setup?.Invoke(app.ApplicationServices, setting);

            return app;
        }

        public static IApplicationBuilder UseAutoInjectJob<T>(this IApplicationBuilder app) where T : IAutoJob
        {
            var jobTypes = RuntimeUtil.AllTypes.Where(x => x.IsBasedOn<T>() && x.IsClass && !x.IsAbstract);
            foreach (var jobType in jobTypes)
            {
                var jobMeta = jobType.GetCustomAttribute<JobMetaAttribute>(false);
                if (jobMeta == null) continue;
                var jobId = jobMeta.JobId ?? jobType.Name;
                var interfaceType = jobType.GetInterfaces()[0];
                var configureMethod = typeof(HangfireApplicationBuilderExtensions)
                    .GetMethod("AddOrUpdateJob", BindingFlags.Public | BindingFlags.Static)
                    .MakeGenericMethod(interfaceType);

                configureMethod.Invoke(null, new object[] { jobId, jobMeta.CronExpression });
            }

            return app;
        }

        public static void AddOrUpdateJob<T>(string jobId, string cronExpression) where T : IAutoJob
        {
            RecurringJob.AddOrUpdate<T>(jobId, x => x.Execute(), cronExpression);
        }
    }
}