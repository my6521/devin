namespace Devin.TestApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpContextAccessor();
            //�Զ�ע�����б�ע����
            builder.Services.AddOptionsInject(builder.Configuration);
            //�Զ�ע�����б�ע����
            builder.Services.RegisterServicesFromAssembly();
            //ͳһ���ݸ�ʽ
            builder.Services.AddControllers().AddResponseWrapper();
            //�汾�Ź���
            builder.Services.AddApiVersion();
            //����ĵ�
            builder.Services.AddSwaggerDocuments(x => builder.Configuration.GetSection("Swagger").Bind(x));
            //���JWT��Ȩ
            builder.Services.AddJwt(x => builder.Configuration.GetSection("JWT").Bind(x));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerDocuments();
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