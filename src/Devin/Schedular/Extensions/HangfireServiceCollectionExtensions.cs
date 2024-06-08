using Devin.Schedular.Filters;
using Devin.Schedular.Options;
using Hangfire;
using Hangfire.Redis;
using StackExchange.Redis;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HangfireServiceCollectionExtensions
    {
        public static IServiceCollection AddHangfireSetup(this IServiceCollection services, Action<HangfireConfig> configureSetup)
        {
            var setting = new HangfireConfig();
            configureSetup?.Invoke(setting);
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
            });
            if (setting.JobExpirationTimeout > 0)
            {
                GlobalStateHandlers.Handlers.Add(new SucceededStateExpireHandler(TimeSpan.FromMinutes(setting.JobExpirationTimeout)));
            }
            services.AddHangfireServer(options => { });

            return services;
        }
    }
}