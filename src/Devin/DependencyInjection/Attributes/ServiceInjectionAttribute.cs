namespace Devin.DependencyInjection.Attributes
{
    /// <summary>
    /// 注册标注
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceInjectionAttribute : Attribute
    {
        public ServiceInjectionAttribute()
        {
        }

        /// <summary>
        /// 注册选项
        /// </summary>
        public ServiceInjectionPatterns Pattern { get; set; }

        /// <summary>
        /// 添加服务方式，存在不添加，或继续添加
        /// </summary>
        public ServiceInjectionActions Action { get; set; }
    }
}