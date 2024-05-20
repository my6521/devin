namespace Devin.Authorization
{
    /// <summary>
    /// Action权限验证接口
    /// </summary>
    public interface IActionAuthorizeValidator
    {
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        AuthorizeResult Valid(string[] permission);
    }
}