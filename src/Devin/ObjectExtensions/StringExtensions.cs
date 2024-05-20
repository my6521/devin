using System.Text.RegularExpressions;

namespace Devin.ObjectExtensions
{
    /// <summary>
    /// 字符串扩展类
    /// </summary>
    public static class StringExtensions
    {
        public static int ToInt(this string obj)
        {
            return Convert.ToInt32(obj);
        }

        public static long ToLong(this string obj)
        {
            return Convert.ToInt64(obj);
        }

        public static decimal ToDecimal(this string obj)
        {
            return Convert.ToDecimal(obj);
        }

        public static double ToDouble(this string obj)
        {
            return Convert.ToDouble(obj);
        }

        public static float ToFloat(this string obj)
        {
            return float.Parse(obj);
        }

        public static DateTime ToDateTime(this string obj)
        {
            DateTime dt = Convert.ToDateTime(obj);
            if (dt > DateTime.MinValue && DateTime.MaxValue > dt)
                return dt;

            return DateTime.Now;
        }

        public static bool IsEmpty(this string obj)
        {
            return string.IsNullOrWhiteSpace(obj);
        }

        /// <summary>
        /// 手机号掩码
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string MaskMobile(this string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile))
            {
                throw new ArgumentNullException("mobile is empty");
            }
            if (mobile.Length != 11)
            {
                throw new ArgumentException("mobile's length not equal 11");
            }

            return Regex.Replace(mobile, "(\\d{4})\\d{3}(\\d{4})", "$1***$2");
        }

        /// <summary>
        /// 身份证号掩码
        /// </summary>
        /// <param name="idNumber"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static string MaskIDNumber(this string idNumber)
        {
            if (string.IsNullOrWhiteSpace(idNumber))
            {
                throw new ArgumentNullException("idNumber is empty");
            }
            if (idNumber.Length != 18)
            {
                throw new ArgumentException("idNumber's length not equal 18");
            }

            return Regex.Replace(idNumber, "(\\d{6})\\d{8}(\\d{4})", "$1********$2");
        }
    }
}