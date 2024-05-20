using Devin.ResponseWrapper.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Devin.ResponseWrapper.Filters
{
    /// <summary>
    /// 模型验证
    /// </summary>
    public class ModelInvalidWrapperFilter : IActionFilter
    {
        private readonly IApiResult _responseWrapper;
        private readonly ILogger<ModelInvalidWrapperFilter> _logger;

        public ModelInvalidWrapperFilter(IApiResult responseWrapper, ILoggerFactory loggerFactory)
        {
            _responseWrapper = responseWrapper;
            _logger = loggerFactory.CreateLogger<ModelInvalidWrapperFilter>();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.Result == null && !context.ModelState.IsValid)
            {
                var responseWrapper = _responseWrapper.Error(ResponseWrapperDefaults.ClientErrorCode, string.Join(",", context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                context.Result = new ObjectResult(responseWrapper)
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}