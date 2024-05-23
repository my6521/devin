namespace Devin.ResponseWrapper
{
    public interface IApiResult<TResponse> : IApiResult
    {
        IApiResult<TResponse> Ok(TResponse response);
    }
}