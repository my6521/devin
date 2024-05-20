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
    public static class JWTAuthorizationServiceCollectionExtensions
    {
        /// <summary>
        /// 添加JWT授权
        /// </summary>
        /// <param name="services"></param>
        /// <param name="jwtSettingConfiure"></param>
        /// <param name="authenticationConfigure"></param>
        /// <param name="jwtBearerConfigure"></param>
        /// <returns></returns>
        public static IServiceCollection AddJwt(this IServiceCollection services, Action<JWTSettingsOptions> jwtSettingConfiure, Action<AuthenticationOptions> authenticationConfigure = null, Action<JwtBearerOptions> jwtBearerConfigure = null)
        {
            var settings = new JWTSettingsOptions();
            jwtSettingConfiure?.Invoke(settings);

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
                    ValidIssuer = settings.ValidIssuer,
                    ValidateIssuerSigningKey = settings.ValidateIssuerSigningKey ?? true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.IssuerSigningKey)),
                    ValidateIssuer = settings.ValidateIssuer ?? true,
                    ValidateAudience = settings.ValidateAudience ?? false,
                    ValidAudience = settings.ValidAudience,
                    ValidateLifetime = settings.ValidateLifetime ?? true,
                    ClockSkew = TimeSpan.FromSeconds(settings.ClockSkew ?? 0)
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

            services.AddSingleton(settings);
            services.AddScoped<JwtHandler>();

            return services;
        }
    }
}