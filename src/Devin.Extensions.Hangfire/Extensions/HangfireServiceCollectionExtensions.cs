using Devin.Extensions.Hangfire.Filters;
using Devin.Extensions.Hangfire.Options;
using Devin.Options;
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
        /// <param name="hangfireConfigSetup"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddHangfireSetup(this IServiceCollection services, Action<IServiceProvider, IGlobalConfiguration>? hangfireConfigSetup = default)
        {
            var setting = OptionsContainer.GetOptions<HangfireOptions>();

            return services.AddHangfireSetup(setting, hangfireConfigSetup);
        }

        /// <summary>
        /// hangfire主要服务配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureSetup"></param>
        /// <param name="hangfireConfigSetup"></param>
        /// <returns></returns>
        public static IServiceCollection AddHangfireSetup(this IServiceCollection services, Action<HangfireOptions> configureSetup, Action<IServiceProvider, IGlobalConfiguration>? hangfireConfigSetup = default)
        {
            var setting = new HangfireOptions();
            configureSetup?.Invoke(setting);

            return services.AddHangfireSetup(setting, hangfireConfigSetup);
        }

        /// <summary>
        /// hangfire主要服务配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setting"></param>
        /// <param name="hangfireConfigSetup"></param>
        /// <returns></returns>
        public static IServiceCollection AddHangfireSetup(this IServiceCollection services, HangfireOptions setting, Action<IServiceProvider, IGlobalConfiguration>? hangfireConfigSetup = default)
        {
            if (setting == null)
                throw new ArgumentNullException(nameof(HangfireOptions));

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

            //过滤器
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail });
            if (setting.JobExpirationTimeout > 0)
                GlobalStateHandlers.Handlers.Add(new SucceededStateExpireHandler(TimeSpan.FromMinutes(setting.JobExpirationTimeout)));

            //扫描任务
            if (setting.AutoScanAndStart)
                JobTypeConfig.GlobalSettings.Scan(setting.Queues);

            services.AddHangfireServer(options =>
            {
                options.Queues = setting.Queues;
            });

            return services;
        }
    }
}