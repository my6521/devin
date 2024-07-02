using Devin.FriendlyException;
using Devin.ResponseWrapper.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Devin.ResponseWrapper.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IApiResult _apiResult;
        private readonly IOptions<MvcNewtonsoftJsonOptions> _mvcJsonOptions;

        public GlobalExceptionMiddleware(RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger,
            IOptions<ResponseWrapperOptions> responseWrapperOptions,
            IOptions<MvcNewtonsoftJsonOptions> mvcJsonOptions)
        {
            _next = next;
            _logger = logger;
            _apiResult = responseWrapperOptions.Value.ResponseWrapper;
            _mvcJsonOptions = mvcJsonOptions;
        }

        /// <summary>
        /// 中间件执行方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            Exception exception = null;
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                if (exception != null)
                {
                    await HandleExceptionAsync(context, exception);
                }
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, "全局异常！！！");
            if (ex is AppFriendlyException appFriendlyException)
            {
                var errorResult = _apiResult.Error((int)appFriendlyException.ErrorCode, ex.Message);
                context.Response.Clear();
                context.Response.ContentType = "application/json;charset=utf-8";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResult, _mvcJsonOptions.Value.SerializerSettings));
            }
            else
            {
                throw ex;
            }
        }
    }
}