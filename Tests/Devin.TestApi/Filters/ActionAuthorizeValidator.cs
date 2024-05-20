using Devin.Authorization;
using Devin.DependencyInjection.Dependencies;

namespace Devin.TestApi.Filters
{
    public class ActionAuthorizeValidator : IActionAuthorizeValidator, IScoped
    {
        public AuthorizeResult Valid(string[] permission)
        {
            return new AuthorizeResult
            {
                IsSuccess = true,
            };
        }
    }
}