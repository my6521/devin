using Devin.DependencyInjection.Attributes;
using Devin.DependencyInjection.Dependencies;
using Devin.Reflection;
using Devin.Utitlies;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 依赖注入拓展类
    /// </summary>
    public static class ServicesInjectionServiceCollectionExtensions
    {
        /// <summary>
        /// 注入所有服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterServicesFromAssembly(this IServiceCollection services, params Assembly[] assemblies)
        {
            if (assemblies.Length == 0)
                assemblies = RuntimeUtil.AllAssemblies.ToArray();

            //所有能注册类型
            var injectTypes = assemblies
                            .SelectMany(assembly => assembly.ExportedTypes
                            .Where(type => type.IsBasedOn<IPrivateDependency>() && type.IsClass && !type.IsInterface && !type.IsAbstract));

            //生命周期类型
            var lifetimeInterfaces = new[] { typeof(ITransient), typeof(IScoped), typeof(ISingleton) };

            foreach (var type in injectTypes)
            {
                // 获取注册方式
                var injectionAttribute = type.GetCustomAttribute<ServiceInjectionAttribute>() ?? new ServiceInjectionAttribute();

                var interfaces = type.GetInterfaces();

                // 获取所有能注册的接口
                var canInjectInterfaces = interfaces.Where(u => u != typeof(IDisposable)
                           && u != typeof(IAsyncDisposable)
                           && u != typeof(IPrivateDependency)
                           && !lifetimeInterfaces.Contains(u)
                           && (
                               !type.IsGenericType && !u.IsGenericType
                               || type.IsGenericType && u.IsGenericType && type.GetGenericArguments().Length == u.GetGenericArguments().Length)
                           );

                // 获取生存周期类型
                var dependencyType = interfaces.Last(u => lifetimeInterfaces.Contains(u));

                //注册类型
                RegisterServices(services, dependencyType, type, injectionAttribute, canInjectInterfaces);
            }

            return services;
        }

        private static void RegisterServices(IServiceCollection services, Type dependencyType, Type type, ServiceInjectionAttribute injectionAttribute, IEnumerable<Type> canInjectInterfaces)
        {
            // 注册自己
            if (injectionAttribute.Pattern is ServiceInjectionPatterns.Self or ServiceInjectionPatterns.All or ServiceInjectionPatterns.SelfWithFirstInterface)
            {
                Register(services, dependencyType, type, injectionAttribute);
            }

            //如果没有接口
            if (!canInjectInterfaces.Any()) return;

            // 只注册第一个接口
            if (injectionAttribute.Pattern is ServiceInjectionPatterns.FirstInterface or ServiceInjectionPatterns.SelfWithFirstInterface)
            {
                Register(services, dependencyType, type, injectionAttribute, canInjectInterfaces.First());
            }
            // 注册多个接口
            else if (injectionAttribute.Pattern is ServiceInjectionPatterns.ImplementedInterfaces or ServiceInjectionPatterns.All)
            {
                foreach (var inter in canInjectInterfaces)
                {
                    Register(services, dependencyType, type, injectionAttribute, inter);
                }
            }
        }

        private static void Register(IServiceCollection services, Type dependencyType, Type type, ServiceInjectionAttribute injectionAttribute, Type inter = null)
        {
            var fixedType = FixedGenericType(type);
            var fixedInter = inter == null ? null : FixedGenericType(inter);
            var lifetime = TryGetServiceLifetime(dependencyType);
            switch (injectionAttribute.Action)
            {
                case ServiceInjectionActions.Add:
                    if (fixedInter == null) services.Add(ServiceDescriptor.Describe(fixedType, fixedType, lifetime));
                    else services.Add(ServiceDescriptor.Describe(fixedInter, fixedType, lifetime));
                    break;

                case ServiceInjectionActions.TryAdd:
                    if (fixedInter == null) services.TryAdd(ServiceDescriptor.Describe(fixedType, fixedType, lifetime));
                    else services.Add(ServiceDescriptor.Describe(fixedInter, fixedType, lifetime));
                    break;
            }
        }

        private static ServiceLifetime TryGetServiceLifetime(Type dependencyType)
        {
            if (dependencyType == typeof(ITransient))
            {
                return ServiceLifetime.Transient;
            }
            else if (dependencyType == typeof(IScoped))
            {
                return ServiceLifetime.Scoped;
            }
            else if (dependencyType == typeof(ISingleton))
            {
                return ServiceLifetime.Singleton;
            }
            else
            {
                throw new InvalidCastException("Invalid service registration lifetime.");
            }
        }

        private static Type FixedGenericType(Type type)
        {
            if (!type.IsGenericType) return type;

            return Reflect.GetType(type.Assembly, $"{type.Namespace}.{type.Name}");
        }
    }
}