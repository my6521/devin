using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Devin.Authorization.Filters
{
    /// <summary>
    /// Action权限认证
    /// </summary>
    public class ActionAuthorizeFilter : IAuthorizationFilter
    {
        private readonly string[] _permission;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="permission"></param>
        public ActionAuthorizeFilter(string[] permission)
        {
            _permission = permission;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="Exception"></exception>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var endpoint = context.HttpContext.Features.Get<IEndpointFeature>()?.Endpoint;
            if (endpoint != null && endpoint.Metadata.GetMetadata<AllowAnonymousAttribute>() != null)
            {
                return;
            }
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new StatusCodeResult(401);
                return;
            }

            var validator = context.HttpContext.RequestServices.GetService<IActionAuthorizeValidator>();

            if (validator == null)
            {
                throw new Exception("权限验证失败：未找到验证接口");
            }

            var validResult = validator.Valid(_permission);
            if (!validResult.IsSuccess)
            {
                throw new Exception($"权限验证失败：{validResult.Error}");
            }
        }
    }
}