using Devin.FriendlyException.Attributes;

namespace Devin.TestApi
{
    [ErrorCodeType]
    public enum ErrorCodes
    {
        [ErrorCodeItemMetadata("未知错误", ErrorCode = 100)]
        None,

        [ErrorCodeItemMetadata("网络错误", ErrorCode = 101)]
        NetworkError,

        [ErrorCodeItemMetadata("超时错误", ErrorCode = 102)]
        Timeout,
    }
}