namespace Devin.ResponseWrapper
{
    /// <summary>
    /// 统一数据泛型接口
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public interface IApiResult<TResponse> : IApiResult
    {
        /// <summary>
        /// 成功返回
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        IApiResult<TResponse> Ok(TResponse response);
    }
}