namespace Devin.ResponseWrapper
{
    public interface IApiResult
    {
        IApiResult Ok();

        IApiResult Error(int code, string message);
    }
}