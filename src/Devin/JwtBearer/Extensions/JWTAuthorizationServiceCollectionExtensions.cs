using Devin.JwtBearer;
using Devin.JwtBearer.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// JWT授权服务拓展类
    /// </summary>
    public static class JwtAuthorizationServiceCollectionExtensions
    {
        /// <summary>
        /// 添加JWT授权
        /// </summary>
        /// <param name="services"></param>
        /// <param name="jwtSettingConfiure"></param>
        /// <param name="authenticationConfigure"></param>
        /// <param name="jwtBearerConfigure"></param>
        /// <returns></returns>
        public static IServiceCollection AddJwt(this IServiceCollection services,
            Action<JwtSettingsOptions> jwtSettingConfiure,
            Action<AuthenticationOptions> authenticationConfigure = null,
            Action<JwtBearerOptions> jwtBearerConfigure = null)
        {
            var jwtSettings = new JwtSettingsOptions();
            jwtSettingConfiure?.Invoke(jwtSettings);

            return services.AddJwt(jwtSettings, authenticationConfigure, jwtBearerConfigure); ;
        }

        /// <summary>
        /// 添加JWT授权
        /// </summary>
        /// <param name="services"></param>
        /// <param name="jwtSettings"></param>
        /// <param name="authenticationConfigure"></param>
        /// <param name="jwtBearerConfigure"></param>
        /// <returns></returns>
        public static IServiceCollection AddJwt(this IServiceCollection services,
            JwtSettingsOptions jwtSettings,
            Action<AuthenticationOptions> authenticationConfigure = null,
            Action<JwtBearerOptions> jwtBearerConfigure = null)
        {
            SetDefaultJwtSettings(jwtSettings);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                // 添加自定义配置
                authenticationConfigure?.Invoke(options);
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // 验证签发方密钥
                    ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey.Value,
                    // 签发方密钥
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.IssuerSigningKey)),
                    // 验证签发方
                    ValidateIssuer = jwtSettings.ValidateIssuer.Value,
                    // 设置签发方
                    ValidIssuer = jwtSettings.ValidIssuer,
                    // 验证签收方
                    ValidateAudience = jwtSettings.ValidateAudience.Value,
                    // 设置接收方
                    ValidAudience = jwtSettings.ValidAudience,
                    // 验证生存期
                    ValidateLifetime = jwtSettings.ValidateLifetime.Value,
                    // 过期时间容错值
                    ClockSkew = TimeSpan.FromSeconds(jwtSettings.ClockSkew.Value),
                    // 验证过期时间，设置 false 永不过期
                    RequireExpirationTime = jwtSettings.RequireExpirationTime
                };

                options.Events = new JwtBearerEvents()
                {
                    OnTokenValidated = (context) =>
                    {
                        return Task.CompletedTask;
                    },
                };

                // 添加自定义配置
                jwtBearerConfigure?.Invoke(options);
            });

            services.AddSingleton(jwtSettings);
            services.AddScoped<JwtHandler>();

            return services;
        }

        internal static JwtSettingsOptions SetDefaultJwtSettings(JwtSettingsOptions options)
        {
            options.ValidateIssuerSigningKey ??= true;
            if (options.ValidateIssuerSigningKey == true)
            {
                options.IssuerSigningKey ??= "U2FsdGVkX1+6H3D8Q//yQMhInzTdRZI9DbUGetbyaag=";
            }
            options.ValidateIssuer ??= true;
            if (options.ValidateIssuer == true)
            {
                options.ValidIssuer ??= "dotnetchina";
            }
            options.ValidateAudience ??= true;
            if (options.ValidateAudience == true)
            {
                options.ValidAudience ??= "powerby Furion";
            }
            options.ValidateLifetime ??= true;
            if (options.ValidateLifetime == true)
            {
                options.ClockSkew ??= 10;
            }
            options.ExpiredTime ??= 20;
            options.Algorithm ??= SecurityAlgorithms.HmacSha256;

            return options;
        }
    }
}