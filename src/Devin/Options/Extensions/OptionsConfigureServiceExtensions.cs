﻿using Devin.Options;
using Devin.Options.Attributes;
using Devin.Reflection;
using Devin.Utitlies;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 配置注册拓展类
    /// </summary>
    public static class OptionsConfigureServiceExtensions
    {
        public static IServiceCollection AddOptionsSetup(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            var allAssemblies = RuntimeUtil.AllAssemblies;
            var allTypes = RuntimeUtil
                .AllAssemblies
                .SelectMany(assembly => assembly.ExportedTypes.Where(t => t.IsBasedOn<IOptionsConfigure>() && t.IsClass && !t.IsAbstract))
                .ToArray();

            services.ConfigureOptions(configuration, allTypes);

            OptionsContainer.Load(configuration);

            return services;
        }

        public static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration, Type[] types)
        {
            foreach (var type in types)
            {
                services.ConfigureOptions(configuration, type);
            }

            return services;
        }

        public static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration, Type type)
        {
            var key = type.GetCustomAttribute<OptionsMetaAttribute>()?.Key ?? type.Name;
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            var configureMethod = typeof(OptionsConfigurationServiceCollectionExtensions)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .First(m => m.Name == "Configure" && m.GetParameters().Length == 2)
                .MakeGenericMethod(type);

            configureMethod.Invoke(null, new object[] { services, configuration.GetSection(key) });

            return services;
        }
    }
}