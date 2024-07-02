using Devin.ResponseWrapper.Internal;

namespace Devin.ResponseWrapper
{
    /// <summary>
    /// 泛型返回数据
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public class ApiResult<TResponse> : ApiResult, IApiResult<TResponse>
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        public TResponse Data { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ApiResult()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code">返回代码</param>
        /// <param name="message">返回消息</param>
        /// <param name="response">返回数据</param>
        private ApiResult(int code, string message, TResponse response) : base(code, message)
        {
            Data = response;
        }

        /// <summary>
        /// 成功返回
        /// </summary>
        /// <param name="response">返回数据</param>
        /// <returns></returns>
        public IApiResult<TResponse> Ok(TResponse response)
        {
            return new ApiResult<TResponse>(ResponseWrapperDefaults.OkCode, null, response);
        }
    }
}