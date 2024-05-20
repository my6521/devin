using System.Security.Claims;

namespace Devin.JwtBearer.Extensions
{
    /// <summary>
    /// claims扩展类
    /// </summary>
    public static class ClaimsIdentityExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="claimType"></param>
        /// <returns></returns>
        public static Claim FindClaim(this ClaimsPrincipal principal, string claimType)
        {
            return principal?.Claims?.FirstOrDefault(c => c.Type == claimType);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="claimType"></param>
        /// <returns></returns>
        public static Claim[] FindClaims(this ClaimsPrincipal principal, string claimType)
        {
            return principal?.Claims?.Where(c => c.Type == claimType).ToArray() ?? Array.Empty<Claim>();
        }
    }
}