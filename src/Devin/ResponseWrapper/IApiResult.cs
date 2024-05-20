namespace Devin.ResponseWrapper
{
    /// <summary>
    /// 接口返回结构
    /// </summary>
    public interface IApiResult
    {
        /// <summary>
        /// 成功
        /// </summary>
        /// <returns></returns>
        IApiResult Ok();

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        IApiResult Error(int code, string message);
    }
}