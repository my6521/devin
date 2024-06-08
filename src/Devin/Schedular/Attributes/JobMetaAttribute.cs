using Devin.Schedular.Enums;

namespace Devin.Schedular.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class JobMetaAttribute : Attribute
    {
        public string JobId { get; set; }
        public JobType JobType { get; set; }
        public string CronExpression { get; set; }
        public string Description { get; set; }
        public string GroupName { get; set; } = "default";
        public int DelaySeconds { get; set; }
    }
}