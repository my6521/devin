namespace Devin.UnitOfWork.FilterAttributes
{
    /// <summary>
    /// 工作单元配置特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class UnitOfWorkAttribute : Attribute
    {
    }
}