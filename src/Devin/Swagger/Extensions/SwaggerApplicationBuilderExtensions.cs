﻿using Devin.Swagger.Internal;
using Devin.Swagger.Options;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;

namespace Microsoft.AspNetCore.Builder
{
    public static class SwaggerApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSwaggerSetup(this IApplicationBuilder app, Action<SwaggerUIOptions> setup = default)
        {
            //获取配置
            var settings = app.ApplicationServices.GetService<SwaggerOptions>();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                //文档标题
                options.DocumentTitle = settings.DocumentTitle;

                //分组设置
                foreach (var group in settings.ApiGroups)
                {
                    options.SwaggerEndpoint($"/swagger/{group.Group}/swagger.json", $"{group.Title}");
                }

                //添加登录验证
                options.AddSwaggerAuth(settings.LoginInfo);

                //添加文档收缩展开
                options.DocExpansion(settings.DocExpansionState);

                //外部设置
                setup?.Invoke(options);
            });

            return app;
        }

        private static SwaggerUIOptions AddSwaggerAuth(this SwaggerUIOptions swaggerUIOptions, SwaggerLoginInfo loginInfo)
        {
            var thisAssembly = typeof(SwaggerApplicationBuilderExtensions).Assembly;
            var customIndex = "Devin.Swagger.Assets.index.html";
            swaggerUIOptions.IndexStream = () =>
            {
                using (var stream = thisAssembly.GetManifestResourceStream(customIndex))
                {
                    using var reader = new StreamReader(stream);
                    var htmlBuilder = new StringBuilder(reader.ReadToEnd());

                    var byteArray = Encoding.UTF8.GetBytes(htmlBuilder.ToString());
                    return new MemoryStream(byteArray);
                }
            };

            swaggerUIOptions.ConfigObject.AdditionalItems.Add("LoginInfo", loginInfo);

            return swaggerUIOptions;
        }
    }
}