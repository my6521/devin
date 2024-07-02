using Devin.Options.Provider;

namespace Devin.TestApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            OptionsProvider.Load(builder.Configuration);
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
            builder.Services.AddSwagger();
            //���JWT��Ȩ
            builder.Services.AddJwt();
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