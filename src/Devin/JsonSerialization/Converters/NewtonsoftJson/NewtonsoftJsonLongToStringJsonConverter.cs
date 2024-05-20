using Newtonsoft.Json;

namespace Devin.JsonSerialization.Converters.NewtonsoftJson
{
    public class NewtonsoftJsonLongToStringJsonConverter : JsonConverter
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NewtonsoftJsonLongToStringJsonConverter()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="overMaxLengthOf17"></param>
        public NewtonsoftJsonLongToStringJsonConverter(bool overMaxLengthOf17 = false)
        {
            OverMaxLengthOf17 = overMaxLengthOf17;
        }

        /// <summary>
        /// 是否超过最大长度 17 再处理
        /// </summary>
        public bool OverMaxLengthOf17 { get; set; }

        /// <summary>
        /// 是否支持反序列化
        /// </summary>
        public override bool CanRead => false;

        /// <summary>
        /// 支持类型
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(long) || objectType == typeof(long?);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false.The type will skip the converter.");
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value != null)
            {
                if (OverMaxLengthOf17)
                {
                    if (value?.ToString()?.Length <= 17) writer.WriteValue(value);
                    else writer.WriteValue(value?.ToString());
                }
                else writer.WriteValue(value?.ToString());
            }
        }
    }
}