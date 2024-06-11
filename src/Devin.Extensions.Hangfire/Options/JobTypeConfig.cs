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
        /// <param name="queue"></param>
        /// <param name="assemblies"></param>
        public void Scan(string[] queue, params Assembly[] assemblies)
        {
            if (!assemblies.Any())
            {
                assemblies = RuntimeUtil.AllAssemblies.ToArray();
            }
            var jobTypes = assemblies.SelectMany(x => x.ExportedTypes).Where(x => x.IsBasedOn<IBaseJob>() && x.HasAttribute<JobMetaAttribute>() && x.IsClass && !x.IsAbstract).ToList();
            foreach (var jobType in jobTypes)
            {
                var jobMetaAttr = jobType.GetCustomAttribute<JobMetaAttribute>(false);
                if (jobMetaAttr != null && queue.Contains(jobMetaAttr.QueueName))
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