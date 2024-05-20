using Devin.SwaggerDocument.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SwaggerDocumentServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerDocuments(this IServiceCollection services, Action<SwaggerDocumentSettingsOptions> configure = default, Action<SwaggerGenOptions> setupAction = default)
        {
            var setting = new SwaggerDocumentSettingsOptions();
            configure?.Invoke(setting);

            services.Configure(configure);

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                //分组
                if (setting.ApiGroups != null)
                {
                    setting.ApiGroups.ForEach(g =>
                    {
                        options.SwaggerDoc(g.Group, new OpenApiInfo
                        {
                            Version = g.Version,
                            Title = g.Title,
                            Description = g.Description
                        });
                    });
                    options.DocInclusionPredicate((docName, apiDescription) =>
                    {
                        if (docName == setting.DefaultGroupName)
                        {
                            return string.IsNullOrEmpty(apiDescription.GroupName);
                        }
                        else if (docName == setting.AllGroupName)
                        {
                            return true;
                        }
                        else
                        {
                            return docName == apiDescription.GroupName;
                        }
                    });
                }

                //外部设置
                setupAction?.Invoke(options);
            });

            return services;
        }
    }
}