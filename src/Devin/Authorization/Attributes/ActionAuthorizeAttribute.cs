using Devin.Authorization.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Devin.Authorization.Attributes
{
    /// <summary>
    /// Action权限标注
    /// </summary>
    public class ActionAuthorizeAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="permission">权限标识，多个使用逗号分割</param>
        public ActionAuthorizeAttribute(params string[] permission) : base(typeof(ActionAuthorizeFilter))
        {
            if (!permission.Any())
            {
                throw new ArgumentException(nameof(permission));
            }
            Arguments = new object[] { permission };
        }
    }
}