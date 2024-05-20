using Devin.ObjectExtensions;
using Newtonsoft.Json;

namespace Devin.JsonSerialization.Converters.NewtonsoftJson
{
    /// <summary>
    /// 身份证掩码
    /// </summary>
    public class NewtonsoftJsonIDNumberToMaskStringConverter : JsonConverter
    {
        public override bool CanRead => false;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false.The type will skip the converter.");
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value != null)
            {
                writer.WriteValue(value?.ToString()?.MaskIDNumber());
            }
        }
    }
}