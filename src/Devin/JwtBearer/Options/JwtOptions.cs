using Devin.Options;

namespace Devin.JwtBearer.Options
{
    /// <summary>
    /// Jwt配置
    /// </summary>
    public class JwtOptions : IOptionsInit
    {
        /// <summary>
        /// 验证签发方密钥
        /// </summary>
        public bool? ValidateIssuerSigningKey { get; set; }

        /// <summary>
        /// 签发方密钥
        /// </summary>
        public string IssuerSigningKey { get; set; }

        /// <summary>
        /// 验证签发方
        /// </summary>
        public bool? ValidateIssuer { get; set; }

        /// <summary>
        /// 签发方
        /// </summary>
        public string ValidIssuer { get; set; }

        /// <summary>
        /// 验证签收方
        /// </summary>
        public bool? ValidateAudience { get; set; }

        /// <summary>
        /// 签收方
        /// </summary>
        public string ValidAudience { get; set; }

        /// <summary>
        /// 验证生存期
        /// </summary>
        public bool? ValidateLifetime { get; set; }

        /// <summary>
        /// 过期时间容错值，解决服务器端时间不同步问题（秒）
        /// </summary>
        public long? ClockSkew { get; set; }

        /// <summary>
        /// 过期时间（分钟）
        /// </summary>
        public int? ExpiredTime { get; set; }

        public string Algorithm { get; set; }
        public bool RequireExpirationTime { get; set; }
    }
}