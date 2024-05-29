using Microsoft.AspNetCore.Http;

namespace Devin.FriendlyException.Extensions
{
    /// <summary>
    /// 异常拓展
    /// </summary>
    public static class AppFriendlyExceptionExtensions
    {
        /// <summary>
        /// 设置异常状态码
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static AppFriendlyException StatusCode(this AppFriendlyException exception, int statusCode = StatusCodes.Status500InternalServerError)
        {
            exception.StatusCode = statusCode;
            return exception;
        }

        /// <summary>
        /// 设置额外数据
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static AppFriendlyException WithData(this AppFriendlyException exception, object data)
        {
            exception.Data = data;
            return exception;
        }
    }
}