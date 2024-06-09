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
        public static IApplicationBuilder UseHangfireCore(this IApplicationBuilder app)
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

            return app;
        }
    }
}