using Devin.Options.Attributes;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class TypeExtensions
    {
        public static string GetOptionsKey(this Type type)
        {
            return type.GetCustomAttribute<OptionsInjectionAttribute>()?.Key ?? type.Name;
        }
    }
}