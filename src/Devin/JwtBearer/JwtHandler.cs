using Devin.JwtBearer.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Devin.JwtBearer
{
    /// <summary>
    /// JWT处理类
    /// </summary>
    public class JwtHandler
    {
        private static readonly string[] _refreshTokenClaims = new[] { "f", "e", "s", "l", "k" };
        private readonly JwtSettingsOptions _jwtSettings;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jwtSettings"></param>
        public JwtHandler(JwtSettingsOptions jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }

        /// <summary>
        /// 生成 Token
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="expiredTime">过期时间（分钟），最大支持 13 年</param>
        /// <returns></returns>
        public string Encrypt(IDictionary<string, object> payload, long? expiredTime = null)
        {
            var (Payload, JWTSettings) = CombinePayload(payload, expiredTime);
            return Encrypt(JWTSettings.IssuerSigningKey, Payload, JWTSettings.Algorithm);
        }

        /// <summary>
        /// 生成 Token
        /// </summary>
        /// <param name="issuerSigningKey"></param>
        /// <param name="payload"></param>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        public string Encrypt(string issuerSigningKey, IDictionary<string, object> payload, string algorithm = SecurityAlgorithms.HmacSha256)
        {
            // 处理 JwtPayload 序列化不一致问题
            var stringPayload = payload is JwtPayload jwtPayload ? jwtPayload.SerializeToJson() : JsonSerializer.Serialize(payload, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            return Encrypt(issuerSigningKey, stringPayload, algorithm);
        }

        /// <summary>
        /// 生成 Token
        /// </summary>
        /// <param name="issuerSigningKey"></param>
        /// <param name="payload"></param>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        public string Encrypt(string issuerSigningKey, string payload, string algorithm = SecurityAlgorithms.HmacSha256)
        {
            SigningCredentials credentials = null;

            if (!string.IsNullOrWhiteSpace(issuerSigningKey))
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(issuerSigningKey));
                credentials = new SigningCredentials(securityKey, algorithm);
            }

            var tokenHandler = new JsonWebTokenHandler();
            return credentials == null ? tokenHandler.CreateToken(payload) : tokenHandler.CreateToken(payload, credentials);
        }

        /// <summary>
        /// 生成刷新 Token
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="expiredTime">刷新 Token 有效期（分钟），最大支持 13 年</param>
        /// <returns></returns>
        public string GenerateRefreshToken(string accessToken, int expiredTime = 43200)
        {
            // 分割Token
            var tokenParagraphs = accessToken.Split('.', StringSplitOptions.RemoveEmptyEntries);

            var s = RandomNumberGenerator.GetInt32(10, tokenParagraphs[1].Length / 2 + 2);
            var l = RandomNumberGenerator.GetInt32(3, 13);

            var payload = new Dictionary<string, object>
            {
                { "f",tokenParagraphs[0] },
                { "e",tokenParagraphs[2] },
                { "s",s },
                { "l",l },
                { "k",tokenParagraphs[1].Substring(s,l) }
            };

            return Encrypt(payload, expiredTime);
        }

        /// <summary>
        /// 读取 Token，不含验证
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static JsonWebToken ReadJwtToken(string accessToken)
        {
            var tokenHandler = new JsonWebTokenHandler();
            if (tokenHandler.CanReadToken(accessToken))
            {
                return tokenHandler.ReadJsonWebToken(accessToken);
            }

            return default;
        }

        /// <summary>
        /// 读取 Token，不含验证
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static JwtSecurityToken SecurityReadJwtToken(string accessToken)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(accessToken);
            return jwtSecurityToken;
        }

        /// <summary>
        /// 验证 Token
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public (bool IsValid, JsonWebToken Token, TokenValidationResult validationResult) Validate(string accessToken)
        {
            // 加密Key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.IssuerSigningKey));
            var creds = new SigningCredentials(key, _jwtSettings.Algorithm);

            // 创建Token验证参数
            var tokenValidationParameters = CreateTokenValidationParameters();
            tokenValidationParameters.IssuerSigningKey ??= creds.Key;

            // 验证 Token
            var tokenHandler = new JsonWebTokenHandler();
            try
            {
                var tokenValidationResult = tokenHandler.ValidateToken(accessToken, tokenValidationParameters);
                if (!tokenValidationResult.IsValid) return (false, null, tokenValidationResult);

                var jsonWebToken = tokenValidationResult.SecurityToken as JsonWebToken;
                return (true, jsonWebToken, tokenValidationResult);
            }
            catch
            {
                return (false, default, default);
            }
        }

        /// <summary>
        /// 生成Token验证参数
        /// </summary>
        /// <returns></returns>
        private TokenValidationParameters CreateTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                // 验证签发方密钥
                ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey.Value,
                // 签发方密钥
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.IssuerSigningKey)),
                // 验证签发方
                ValidateIssuer = _jwtSettings.ValidateIssuer.Value,
                // 设置签发方
                ValidIssuer = _jwtSettings.ValidIssuer,
                // 验证签收方
                ValidateAudience = _jwtSettings.ValidateAudience.Value,
                // 设置接收方
                ValidAudience = _jwtSettings.ValidAudience,
                // 验证生存期
                ValidateLifetime = _jwtSettings.ValidateLifetime.Value,
                // 过期时间容错值
                ClockSkew = TimeSpan.FromSeconds(_jwtSettings.ClockSkew.Value),
                // 验证过期时间，设置 false 永不过期
                RequireExpirationTime = _jwtSettings.RequireExpirationTime
            };
        }

        /// <summary>
        /// 组合 Claims 负荷
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="expiredTime">过期时间，单位：分钟，最大支持 13 年</param>
        /// <returns></returns>
        private (IDictionary<string, object> Payload, JwtSettingsOptions JWTSettings) CombinePayload(IDictionary<string, object> payload, long? expiredTime = null)
        {
            var datetimeOffset = DateTimeOffset.UtcNow;

            if (!payload.ContainsKey(JwtRegisteredClaimNames.Iat))
            {
                payload.Add(JwtRegisteredClaimNames.Iat, datetimeOffset.ToUnixTimeSeconds());
            }

            if (!payload.ContainsKey(JwtRegisteredClaimNames.Nbf))
            {
                payload.Add(JwtRegisteredClaimNames.Nbf, datetimeOffset.ToUnixTimeSeconds());
            }

            if (!payload.ContainsKey(JwtRegisteredClaimNames.Exp))
            {
                var minute = expiredTime ?? _jwtSettings.ExpiredTime ?? 20;
                payload.Add(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddMinutes(minute).ToUnixTimeSeconds());
            }

            if (!payload.ContainsKey(JwtRegisteredClaimNames.Iss))
            {
                payload.Add(JwtRegisteredClaimNames.Iss, _jwtSettings?.ValidIssuer);
            }

            if (!payload.ContainsKey(JwtRegisteredClaimNames.Aud))
            {
                payload.Add(JwtRegisteredClaimNames.Aud, _jwtSettings?.ValidAudience);
            }

            return (payload, _jwtSettings);
        }
    }
}