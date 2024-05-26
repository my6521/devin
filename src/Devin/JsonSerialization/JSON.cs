using System.Text.Json;

namespace Devin.JsonSerialization
{
    /// <summary>
    /// JSON 静态帮助类
    /// </summary>
    public static class JSON
    {
        /// <summary>
        /// 检查 JSON 字符串是否有效
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static bool IsValid(string jsonString)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                return false;
            }

            try
            {
                using (JsonDocument.Parse(jsonString))
                {
                    return true;
                }
            }
            catch (JsonException)
            {
                return false;
            }
        }
    }
}