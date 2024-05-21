using Newtonsoft.Json.Converters;

namespace Devin.JsonSerialization.Converters.NewtonsoftJson
{
    /// <summary>
    /// 转化为日期格式
    /// </summary>
    public class NewtonsoftJsonDateFormatConverter : IsoDateTimeConverter
    {
        public NewtonsoftJsonDateFormatConverter()
        {
            DateTimeFormat = "yyyy-MM-dd";
        }
    }
}