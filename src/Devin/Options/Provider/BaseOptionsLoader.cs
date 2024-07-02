using Devin.Options.Attributes;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Devin.Options.Provider
{
    /// <summary>
    /// 配置加载器基类
    /// </summary>
    public abstract class BaseOptionsLoader : IOptionsLoader
    {
        public void Load(IConfiguration configuration, IDictionary<Type, object> map)
        {
            var type = this.GetType();
            var settings = Activator.CreateInstance(type);
            var propertyInfos = type.GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                if (map.ContainsKey(propertyInfo.PropertyType))
                    throw new Exception($"配置重复加载 type:{propertyInfo.PropertyType.FullName}");

                var attr = propertyInfo.GetCustomAttribute<OptionsMetaAttribute>();
                var key = attr?.Key ?? propertyInfo.Name;
                if (configuration.GetSection(key).Exists())
                {
                    var value = Activator.CreateInstance(propertyInfo.PropertyType);
                    configuration.GetSection(key).Bind(value);
                    propertyInfo.SetValue(settings, value);
                    map[propertyInfo.PropertyType] = value;
                }
            }
        }
    }
}