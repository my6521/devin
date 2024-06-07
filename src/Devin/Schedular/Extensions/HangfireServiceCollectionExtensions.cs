using Devin.Schedular.Options;
using Hangfire;
using Hangfire.Redis.StackExchange;
using StackExchange.Redis;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HangfireServiceCollectionExtensions
    {
        public static IServiceCollection AddHangfireSetup(this IServiceCollection services, Action<HangfireConfig> configureSetup)
        {
            var hangfireConfig = new HangfireConfig();
            configureSetup?.Invoke(hangfireConfig);
            services.AddSingleton(hangfireConfig);

            services.AddHangfire((sp, config) =>
            {
                var redis = ConnectionMultiplexer.Connect(hangfireConfig.ConnectionString);
                config.UseRedisStorage(redis, new RedisStorageOptions
                {
                    Db = hangfireConfig.Db,
                    SucceededListSize = 1000,
                    DeletedListSize = 1000,
                    Prefix = hangfireConfig.Prefix
                });
            });

            services.AddHangfireServer(options => { });

            return services;
        }
    }
}