using Devin.ResponseWrapper.Middlewares;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 统一返回格式
    /// </summary>
    public static class ResponseWrapperApplicationBuilderExtensions
    {
        /// <summary>
        /// 统一返回格式
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseResponseWrapper(this IApplicationBuilder builder)
        {
            // 全局异常中间件
            builder.UseMiddleware<GlobalExceptionMiddleware>();

            return builder;
        }
    }
}