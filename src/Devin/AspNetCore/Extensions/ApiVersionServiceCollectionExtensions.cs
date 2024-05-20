using Asp.Versioning;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApiVersionServiceCollectionExtensions
    {
        public static IServiceCollection AddApiVersion(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                config.ReportApiVersions = true;
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader(), new HeaderApiVersionReader("api-version"));
            })
            .AddApiExplorer(config =>
            {
                config.SubstituteApiVersionInUrl = true;
                config.GroupNameFormat = "'V'VVV";
                config.AssumeDefaultVersionWhenUnspecified = true;
            });

            return services;
        }
    }
}