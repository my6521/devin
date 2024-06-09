using Devin.Extensions.Hangfire.Attributes;
using Devin.Extensions.Hangfire.Internal;
using Devin.Reflection;
using Devin.Utitlies;
using System.Collections.Concurrent;
using System.Reflection;

namespace Devin.Extensions.Hangfire.Options
{
    /// <summary>
    /// 作业程序集配置
    /// </summary>
    public class JobTypeConfig
    {
        private ConcurrentDictionary<Type, JobMetaAttribute> _jobTypeDict = new ConcurrentDictionary<Type, JobMetaAttribute>();

        /// <summary>
        /// 单列
        /// </summary>
        public static JobTypeConfig GlobalSettings { get; } = new JobTypeConfig();

        /// <summary>
        /// 扫描作业类型
        /// </summary>
        /// <typeparam name="T">特性标注。一般根据应用自动扫码该特性的作业类型</typeparam>
        /// <param name="assemblies"></param>
        public void Scan<T>(params Assembly[] assemblies) where T : Attribute
        {
            if (!assemblies.Any())
            {
                assemblies = RuntimeUtil.AllAssemblies.ToArray();
            }
            var jobTypes = assemblies.SelectMany(x => x.ExportedTypes).Where(x => x.IsBasedOn<IBaseJob>() && x.HasAttribute<T>() && x.IsClass && !x.IsAbstract).ToList();
            foreach (var jobType in jobTypes)
            {
                var jobMetaAttr = jobType.GetCustomAttribute<JobMetaAttribute>(false);
                if (jobMetaAttr != null)
                    _jobTypeDict.TryAdd(jobType, jobMetaAttr);
            }
        }

        /// <summary>
        /// 获取作业程序集
        /// </summary>
        public ConcurrentDictionary<Type, JobMetaAttribute> JobTypeDic
        {
            get
            {
                return _jobTypeDict;
            }
        }
    }
}