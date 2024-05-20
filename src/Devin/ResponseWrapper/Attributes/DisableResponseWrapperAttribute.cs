namespace Devin.ResponseWrapper.Attributes
{
    /// <summary>
    /// 忽略接口统一数据结构
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class DisableResponseWrapperAttribute : Attribute
    {
    }
}