using Devin.Extensions.Hangfire.Enums;

namespace Devin.Extensions.Hangfire.Attributes
{
    /// <summary>
    /// 任务描述
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class JobMetaAttribute : Attribute
    {
        /// <summary>
        /// 作业ID
        /// </summary>
        public string JobId { get; set; }

        /// <summary>
        /// 队列
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        public BackgroudJobType BackgroudJobType { get; set; }

        /// <summary>
        /// Cron表达式
        /// </summary>
        public string CronExpression { get; set; }

        /// <summary>
        /// 作业描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否自动启动
        /// </summary>
        public bool AutoStart { get; set; } = true;
    }
}