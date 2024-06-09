namespace Devin.Extensions.Hangfire.Enums
{
    /// <summary>
    /// 作业类型枚举
    /// </summary>
    public enum BackgroudJobType
    {
        /// <summary>
        /// 周期性任务
        /// </summary>
        RecurringJob,

        /// <summary>
        /// 后台任务
        /// </summary>
        BackgroundJob,
    }
}