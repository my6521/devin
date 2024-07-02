﻿using Devin.Options.Provider;
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
        /// <param name="setupAction"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddSwaggerSetup(this IServiceCollection services, Action<SwaggerGenOptions> setupAction = default)
        {
            var setting = OptionsProvider.GetOptions<SwaggerOptions>();
            if (setting == null)
                throw new ArgumentNullException(nameof(setting));

            return services.AddSwaggerSetup(setting, setupAction);
        }

        /// <summary>
        /// 添加Swagger文档服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerSetup(this IServiceCollection services, Action<SwaggerOptions> configure, Action<SwaggerGenOptions> setupAction = default)
        {
            var setting = new SwaggerOptions();
            configure?.Invoke(setting);

            return services.AddSwaggerSetup(setting, setupAction);
        }

        /// <summary>
        /// 添加Swagger文档服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setting"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerSetup(this IServiceCollection services, SwaggerOptions setting, Action<SwaggerGenOptions> setupAction = default)
        {
            services.AddSingleton(setting);

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
                setupAction?.Invoke(options);
            });

            return services;
        }
    }
}