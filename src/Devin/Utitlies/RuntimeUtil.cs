using Microsoft.Extensions.DependencyModel;
using System.Reflection;
using System.Runtime.Loader;

namespace Devin.Utitlies
{
    public static class RuntimeUtil
    {
        public static List<Assembly> AllAssemblies { get; private set; }
        public static List<Type> AllTypes { get; private set; }

        static RuntimeUtil()
        {
            AllAssemblies = GetAllAssemblies();
            AllTypes = AllAssemblies.SelectMany(x => x.ExportedTypes).ToList();
        }

        private static List<Assembly> GetAllAssemblies()
        {
            var list = new List<Assembly>();
            var deps = DependencyContext.Default ?? throw new ArgumentException("注入内容获取失败 位置：RuntimeUtil.GetAllAssemblies() 18-Line");
            var libs = deps.CompileLibraries.Where(lib => lib.Type == "project");
            foreach (var lib in libs)
            {
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
                list.Add(assembly);
            }

            return list.Distinct().ToList();
        }
    }
}