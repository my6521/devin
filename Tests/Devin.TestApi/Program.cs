using Devin.Options.Provider;
using Devin.TestApi.Options;

namespace Devin.TestApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var testoptions = OptionsProvider.GetOptions<TestOptions>();

            //��ӻ���
            builder.Services.AddCore();
            //�Զ�ע�����б�ע����
            builder.Services.AddOptionsSetup(builder.Configuration);
            //�Զ�ע�����б�ע����
            builder.Services.RegisterServicesFromAssembly();
            //ͳһ���ݸ�ʽ
            builder.Services.AddControllers().AddResponseWrapper();
            builder.Services.AddObjectMapper();
            //�汾�Ź���
            builder.Services.AddApiVersion();
            //����ĵ�
            builder.Services.AddSwaggerSetup(x => builder.Configuration.GetSection("Swagger").Bind(x));
            //���JWT��Ȩ
            builder.Services.AddJwt(x => builder.Configuration.GetSection("JWT").Bind(x));
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerSetup();
            }

            //ͳһ���ݸ�ʽ
            app.UseResponseWrapper();

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}