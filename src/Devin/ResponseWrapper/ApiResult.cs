using Devin.ResponseWrapper.Internal;

namespace Devin.ResponseWrapper
{
    /// <summary>
    /// 统一数据格式
    /// </summary>
    public class ApiResult : IApiResult
    {
        /// <summary>
        /// 接口返回代码
        /// </summary>
        public int Code { get; }

        /// <summary>
        /// 接口返回消息
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess => Code > 0;

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
        protected ApiResult(int code, string message)
        {
            Code = code;
            Message = message;
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <returns></returns>
        public IApiResult Ok()
        {
            return new ApiResult(ResponseWrapperDefaults.OkCode, "ok");
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public IApiResult Error(int code, string message)
        {
            if (code == 0)
            {
                code = ResponseWrapperDefaults.BusinessErrorCode;
            }

            return new ApiResult(code, message);
        }
    }
}