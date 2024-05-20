using Devin.JwtBearer.Internal;
using Devin.JwtBearer.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Devin.JwtBearer
{
    /// <summary>
    /// JWT处理类
    /// </summary>
    public class JwtHandler
    {
        private readonly JWTSettingsOptions _jwtSettingOptions;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jwtSettingOptions"></param>
        public JwtHandler(JWTSettingsOptions jwtSettingOptions)
        {
            _jwtSettingOptions = jwtSettingOptions;
        }

        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="userId">用户标识</param>
        /// <param name="claimsIdentity">额外</param>
        /// <param name="expiredTime">超时时间</param>
        /// <returns></returns>
        public TokenResult GenerateToken(string userId, ClaimsIdentity claimsIdentity = null, int? expiredTime = null)
        {
            var result = new TokenResult();

            if (claimsIdentity == null) claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(ClaimsConst.UserId, userId));

            var lifetime = expiredTime ?? _jwtSettingOptions.ExpiredTime ?? 120;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtSettingOptions.ValidIssuer,
                Audience = userId,
                Subject = claimsIdentity,
                Expires = DateTime.Now.AddMinutes(lifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettingOptions.IssuerSigningKey)),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            result.AccessToken = tokenHandler.WriteToken(token);
            result.Expire = lifetime;

            return result;
        }

        /// <summary>
        /// 验证Token
        /// </summary>
        /// <param name="token">token字符串</param>
        /// <param name="claimsPrincipal"></param>
        /// <returns></returns>
        public bool ValidateToken(string token, out ClaimsPrincipal claimsPrincipal)
        {
            claimsPrincipal = null;

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _jwtSettingOptions.ValidIssuer,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(0),
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettingOptions.IssuerSigningKey))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            try
            {
                claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}