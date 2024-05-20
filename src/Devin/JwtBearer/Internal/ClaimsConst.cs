using System.Security.Claims;

namespace Devin.JwtBearer.Internal
{
    public class ClaimsConst
    {
        public const string UserId = ClaimTypes.NameIdentifier;
        public const string UserName = ClaimTypes.Name;
        public const string IsSupperAdmin = "IsSupperAdmin";
        public const string Issuer = "Issuer";
    }
}