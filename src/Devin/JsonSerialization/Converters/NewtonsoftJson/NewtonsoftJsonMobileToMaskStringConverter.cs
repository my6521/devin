using Devin.ObjectExtensions;
using Newtonsoft.Json;

namespace Devin.JsonSerialization.Converters.NewtonsoftJson
{
    /// <summary>
    /// 手机掩码
    /// </summary>
    public class NewtonsoftJsonMobileToMaskStringConverter : JsonConverter
    {
        public override bool CanRead => false;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false.The type will skip the converter.");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value != null)
            {
                writer.WriteValue(value?.ToString()?.MaskMobile());
            }
        }
    }
}