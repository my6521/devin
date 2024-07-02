using Devin.JwtBearer;
using Devin.JwtBearer.Options;
using Devin.Options.Provider;
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
        public static IServiceCollection AddJwt(this IServiceCollection services,
            Action<AuthenticationOptions> authenticationConfigure = null,
            Action<JwtBearerOptions> jwtBearerConfigure = null)
        {
            var setting = OptionsProvider.GetOptions<JwtSettingsOptions>();
            if (setting == null)
                throw new ArgumentNullException(nameof(setting));

            return services.AddJwt(setting, authenticationConfigure, jwtBearerConfigure); ;
        }

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
            var setting = new JwtSettingsOptions();
            jwtSettingConfiure?.Invoke(setting);

            return services.AddJwt(setting, authenticationConfigure, jwtBearerConfigure); ;
        }

        /// <summary>
        /// 添加JWT授权
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setting"></param>
        /// <param name="authenticationConfigure"></param>
        /// <param name="jwtBearerConfigure"></param>
        /// <returns></returns>
        public static IServiceCollection AddJwt(this IServiceCollection services,
            JwtSettingsOptions setting,
            Action<AuthenticationOptions> authenticationConfigure = null,
            Action<JwtBearerOptions> jwtBearerConfigure = null)
        {
            SetDefaultJwtSettings(setting);
            services.AddSingleton(setting);

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
                    ValidateIssuerSigningKey = setting.ValidateIssuerSigningKey.Value,
                    // 签发方密钥
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(setting.IssuerSigningKey)),
                    // 验证签发方
                    ValidateIssuer = setting.ValidateIssuer.Value,
                    // 设置签发方
                    ValidIssuer = setting.ValidIssuer,
                    // 验证签收方
                    ValidateAudience = setting.ValidateAudience.Value,
                    // 设置接收方
                    ValidAudience = setting.ValidAudience,
                    // 验证生存期
                    ValidateLifetime = setting.ValidateLifetime.Value,
                    // 过期时间容错值
                    ClockSkew = TimeSpan.FromSeconds(setting.ClockSkew.Value),
                    // 验证过期时间，设置 false 永不过期
                    RequireExpirationTime = setting.RequireExpirationTime
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