namespace Devin.Extensions.Hangfire.Internal
{
    /// <summary>
    /// 作业顶级接口
    /// </summary>
    public interface IBaseJob
    {
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <returns></returns>
        Task ExecuteAsync();
    }
}