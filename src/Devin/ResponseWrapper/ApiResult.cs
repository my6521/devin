using Devin.ResponseWrapper.Internal;

namespace Devin.ResponseWrapper
{
    public class ApiResult : IApiResult
    {
        public int Code { get; }

        public string Message { get; }

        public ApiResult()
        {
        }

        protected ApiResult(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public IApiResult Ok()
        {
            return new ApiResult(ResponseWrapperDefaults.OkCode, null);
        }

        public IApiResult Error(int code, string message)
        {
            return new ApiResult(code, message);
        }
    }
}