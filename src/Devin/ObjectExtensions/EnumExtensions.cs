using System.ComponentModel;
using System.Reflection;

namespace Devin.Utitlies.Extensions
{
    public static class EnumExtensions
    {
        //缓存
        private static Dictionary<Enum, Tuple<int, string>> caches = new Dictionary<Enum, Tuple<int, string>>();

        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        public static Tuple<int, string> GetDescription(this Enum errorCode)
        {
            if (caches.ContainsKey(errorCode))
            {
                return caches[errorCode];
            }

            var name = errorCode.ToString();
            var type = errorCode.GetType();
            var fieldInfo = type.GetField(name);
            var value = ((int)type.InvokeMember(name, BindingFlags.GetField, null, null, null));

            object[] objs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (objs == null || objs.Length == 0)
                return new Tuple<int, string>(value, name);

            var descriptionAttr = (DescriptionAttribute)objs[0];

            caches[errorCode] = new Tuple<int, string>(value, descriptionAttr.Description);

            return caches[errorCode];
        }
    }
}