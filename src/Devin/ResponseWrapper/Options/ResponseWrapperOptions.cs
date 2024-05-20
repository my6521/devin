using Devin.Options.Attributes;

namespace Devin.ResponseWrapper.Options
{
    [IgnoreOptionInjection]
    public class ResponseWrapperOptions
    {
        public IApiResult ResponseWrapper { get; set; } = new ApiResult();

        public IApiResult<object> GenericResponseWrapper { get; set; } = new ApiResult<object>();

        public bool SuppressModelInvalidWrapper { get; set; }

        public bool OnlyAvailableInApiController { get; set; }
    }
}