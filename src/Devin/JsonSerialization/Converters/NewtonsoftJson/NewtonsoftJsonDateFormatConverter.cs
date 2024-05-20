using Newtonsoft.Json.Converters;

namespace Devin.JsonSerialization.Converters.NewtonsoftJson
{
    public class NewtonsoftJsonDateFormatConverter : IsoDateTimeConverter
    {
        public NewtonsoftJsonDateFormatConverter()
        {
            DateTimeFormat = "yyyy-MM-dd";
        }
    }
}