using Devin.Authorization;
using Devin.DependencyInjection.Dependencies;

namespace Devin.TestApi
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