using Devin.JsonSerialization.Converters.NewtonsoftJson;
using Devin.ResponseWrapper.Options;
using Devin.ResponseWrapper.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 统一返回格式
    /// </summary>
    public static class ResponseWrapperBuilderExtensions
    {
        /// <summary>
        /// 添加统一返回格式
        /// </summary>
        /// <param name="mvcBuilder"></param>
        /// <param name="jsonOptionSetupAction"></param>
        /// <returns></returns>
        public static IMvcBuilder AddResponseWrapper(this IMvcBuilder mvcBuilder, Action<MvcNewtonsoftJsonOptions> jsonOptionSetupAction = default)
        {
            return AddResponseWrapper(mvcBuilder, _ => { }, jsonOptionSetupAction);
        }

        /// <summary>
        /// 添加统一返回格式
        /// </summary>
        /// <param name="mvcBuilder"></param>
        /// <param name="optionSetupAction"></param>
        /// <param name="jsonOptionSetupAction"></param>
        /// <returns></returns>
        public static IMvcBuilder AddResponseWrapper(this IMvcBuilder mvcBuilder, Action<ResponseWrapperOptions> optionSetupAction, Action<MvcNewtonsoftJsonOptions> jsonOptionSetupAction = default)
        {
            mvcBuilder.Services.Configure(optionSetupAction);
            mvcBuilder.ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            mvcBuilder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IApplicationModelProvider, ResponseWrapperApplicationModelProvider>());

            //json格式化
            mvcBuilder.AddNewtonsoftJson(c =>
            {
                c.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                c.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                c.SerializerSettings.ContractResolver = new DefaultContractResolver();
                c.SerializerSettings.Converters.Add(new NewtonsoftJsonLongToStringJsonConverter());

                jsonOptionSetupAction?.Invoke(c);
            });

            return mvcBuilder;
        }
    }
}