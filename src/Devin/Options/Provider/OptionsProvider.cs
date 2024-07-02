using Devin.Reflection;
using Devin.Utitlies;
using Microsoft.Extensions.Configuration;

namespace Devin.Options.Provider
{
    /// <summary>
    /// 配置加载器
    /// </summary>
    public static class OptionsProvider
    {
        private static IDictionary<Type, object> _map = new Dictionary<Type, object>();

        public static void Load(IConfiguration configuration)
        {
            var types = RuntimeUtil.AllTypes.Where(t => t.IsBasedOn<IOptionsLoader>() && !t.IsAbstract).ToList();
            foreach (var type in types)
            {
                var loader = (IOptionsLoader)Activator.CreateInstance(type);
                loader.Load(configuration, _map);
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
                return default(T);

            return (T)_map[type];
        }
    }
}