using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Devin.ResponseWrapper.Filters
{
    public class ActionResultWrapperFilter : IActionFilter
    {
        private readonly IApiResult _responseWrapper;
        private readonly IApiResult<object?> _responseWithDataWrapper;

        public ActionResultWrapperFilter(IApiResult responseWrapper, IApiResult<object?> responseWithDataWrapper)
        {
            _responseWrapper = responseWrapper;
            _responseWithDataWrapper = responseWithDataWrapper;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            switch (context.Result)
            {
                case EmptyResult:
                    context.Result = new OkObjectResult(_responseWrapper.Ok());
                    return;

                case ObjectResult objectResult:
                    context.Result = new OkObjectResult(_responseWithDataWrapper.Ok(objectResult.Value));
                    return;
            }
        }
    }
}