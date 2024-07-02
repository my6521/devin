using Devin.Options.Attributes;
using Devin.Reflection;
using Devin.Utitlies;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Devin.Options
{
    /// <summary>
    /// 配置加载器
    /// </summary>
    public static class OptionsContainer
    {
        private static IDictionary<Type, object> _map = new Dictionary<Type, object>();

        public static void Load(IConfiguration configuration)
        {
            var types = RuntimeUtil.AllTypes.Where(t => t.IsBasedOn<IOptionsInit>() && !t.IsAbstract && t.IsClass).ToList();
            foreach (var type in types)
            {
                var options = Activator.CreateInstance(type);
                var attr = type.GetCustomAttribute<OptionsMetaAttribute>();
                var key = attr?.Key ?? type.Name;
                if (configuration.GetSection(key).Exists())
                {
                    configuration.GetSection(key).Bind(options);
                    _map[type] = options;
                }
            }
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <returns></returns>
        public static T GetOptions<T>() where T : class, new()
        {
            var type = typeof(T);
            if (!_map.ContainsKey(type))
                return default;

            return (T)_map[type];
        }
    }
}