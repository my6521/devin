namespace Devin.TestApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //添加基础
            builder.Services.AddCore();
            //自动注入所有标注配置
            builder.Services.AddOptionsSetup(builder.Configuration);
            //自动注入所有标注服务
            builder.Services.RegisterServicesFromAssembly();
            //统一数据格式
            builder.Services.AddControllers().AddResponseWrapper();
            builder.Services.AddObjectMapper();
            //版本号管理
            builder.Services.AddApiVersion();
            //添加文档
            builder.Services.AddSwagger();
            //添加JWT授权
            builder.Services.AddJwt();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerSetup();
            }

            //统一数据格式
            app.UseResponseWrapper();

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}