namespace Devin.ResponseWrapper
{
    public interface IApiResult<in TResponse> : IApiResult
    {
        IApiResult<TResponse> Ok(TResponse response);
    }
}