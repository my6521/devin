using Devin.Options;
using Devin.Swagger.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Swagger拓展
    /// </summary>
    public static class SwaggerServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Swagger文档服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="swaggerGenOptionSetup"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddSwagger(this IServiceCollection services, Action<SwaggerGenOptions> swaggerGenOptionSetup = null)
        {
            var setting = OptionsContainer.GetOptions<SwaggerOptions>();

            return services.AddSwaggerSetup(setting, swaggerGenOptionSetup);
        }

        /// <summary>
        /// 添加Swagger文档服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="swaggerOptionsSetup"></param>
        /// <param name="swaggerGenOptionsSetup"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerSetup(this IServiceCollection services, Action<SwaggerOptions> swaggerOptionsSetup, Action<SwaggerGenOptions> swaggerGenOptionsSetup = null)
        {
            var setting = new SwaggerOptions();
            swaggerOptionsSetup?.Invoke(setting);

            return services.AddSwaggerSetup(setting, swaggerGenOptionsSetup);
        }

        /// <summary>
        /// 添加Swagger文档服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="swaggerOptions"></param>
        /// <param name="swaggerGenOptionsSetup"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerSetup(this IServiceCollection services, SwaggerOptions swaggerOptions, Action<SwaggerGenOptions> swaggerGenOptionsSetup = null)
        {
            if (swaggerOptions == null) throw new ArgumentNullException(nameof(SwaggerOptions));

            services.AddSingleton(swaggerOptions);

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                //分组
                if (swaggerOptions.ApiGroups != null)
                {
                    swaggerOptions.ApiGroups.ForEach(g =>
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
                        if (docName == swaggerOptions.DefaultGroupName)
                        {
                            return string.IsNullOrEmpty(apiDescription.GroupName);
                        }
                        else if (docName == swaggerOptions.AllGroupName)
                        {
                            return true;
                        }
                        else
                        {
                            return docName == apiDescription.GroupName;
                        }
                    });
                }

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "请输入带有Bearer的Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "Bearer"}
                        },
                        new List<string>()
                    }
                });

                //外部设置
                swaggerGenOptionsSetup?.Invoke(options);
            });

            return services;
        }
    }
}