using Devin.ResponseWrapper.Internal;

namespace Devin.ResponseWrapper
{
    public class ApiResult<TResponse> : ApiResult, IApiResult<TResponse>
    {
        public TResponse Data { get; }

        public ApiResult()
        {
        }

        private ApiResult(int code, string message, TResponse data) : base(code, message)
        {
            Data = data;
        }

        public IApiResult<TResponse> Ok(TResponse response)
        {
            return new ApiResult<TResponse>(ResponseWrapperDefaults.OkCode, null, response);
        }
    }
}