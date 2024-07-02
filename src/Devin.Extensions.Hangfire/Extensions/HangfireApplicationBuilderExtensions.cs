using Devin.Extensions.Hangfire;
using Devin.Extensions.Hangfire.Enums;
using Devin.Extensions.Hangfire.Options;
using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// hangfire应用拓展类
    /// </summary>
    public static class HangfireApplicationBuilderExtensions
    {
        /// <summary>
        /// hangfire应用面板
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IApplicationBuilder UseHangfireSetup(this IApplicationBuilder app)
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

            //自动添加任务需要考虑多个集群重复问题
            if (setting.AutoScanAndStart)
                StartJob();

            return app;
        }

        private static void StartJob()
        {
            var jobList = JobTypeConfig.GlobalSettings.JobTypeDic.Where(x => x.Value.AutoStart);
            foreach (var job in jobList)
            {
                //先删除
                RecurringJob.RemoveIfExists(job.Value.JobId);

                if (job.Value.BackgroudJobType == BackgroudJobType.RecurringJob)
                {
                    job.Key.AddOrUpdateRecurringJob(job.Value.JobId, job.Value.QueueName, job.Value.CronExpression);
                }
                else
                {
                    job.Key.AddOrUpdateBackgroundJob(job.Value.QueueName);
                }
            }
        }
    }
}