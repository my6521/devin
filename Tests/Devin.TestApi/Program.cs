namespace Devin.TestApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //�Զ�ע�����б�ע����
            builder.Services.AddOptionsInject(builder.Configuration);
            //�Զ�ע�����б�ע����
            builder.Services.RegisterServicesFromAssembly();
            //ͳһ���ݸ�ʽ
            builder.Services.AddControllers().AddResponseWrapper();
            //�汾�Ź���
            builder.Services.AddApiVersion();
            //����ĵ�
            builder.Services.AddSwaggerDocuments(x => builder.Configuration.GetSection(x.GetType().Name).Bind(x));

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