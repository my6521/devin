using Devin.Extensions.Hangfire.Filters;
using Devin.Extensions.Hangfire.Options;
using Hangfire;
using Hangfire.Redis.StackExchange;
using StackExchange.Redis;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Hangfire服扩展类
    /// </summary>
    public static class HangfireServiceCollectionExtensions
    {
        /// <summary>
        /// hangfire主要服务配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureSetup"></param>
        /// <param name="hangfireConfigSetup"></param>
        /// <returns></returns>
        public static IServiceCollection AddHangfireCore(this IServiceCollection services, Action<HangfireConfig> configureSetup, Action<IServiceProvider, IGlobalConfiguration>? hangfireConfigSetup = default)
        {
            var setting = new HangfireConfig();
            configureSetup?.Invoke(setting);

            return services.AddHangfireCore(setting, hangfireConfigSetup);
        }

        /// <summary>
        /// hangfire主要服务配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setting"></param>
        /// <param name="hangfireConfigSetup"></param>
        /// <returns></returns>
        public static IServiceCollection AddHangfireCore(this IServiceCollection services, HangfireConfig setting, Action<IServiceProvider, IGlobalConfiguration>? hangfireConfigSetup = default)
        {
            setting.Queues ??= new string[] { "default" };
            services.AddSingleton(setting);

            services.AddHangfire((sp, config) =>
            {
                var redis = ConnectionMultiplexer.Connect(setting.ConnectionString);
                config.UseRedisStorage(redis, new RedisStorageOptions
                {
                    Db = setting.Db,
                    SucceededListSize = 1000,
                    DeletedListSize = 1000,
                    Prefix = setting.Prefix
                });
                hangfireConfigSetup?.Invoke(sp, config);
            });
            if (setting.JobExpirationTimeout > 0)
            {
                GlobalStateHandlers.Handlers.Add(new SucceededStateExpireHandler(TimeSpan.FromMinutes(setting.JobExpirationTimeout)));
            }

            services.AddHangfireServer(options =>
            {
                options.Queues = setting.Queues;
            });

            return services;
        }
    }
}