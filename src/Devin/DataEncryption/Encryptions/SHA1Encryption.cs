using System.Security.Cryptography;
using System.Text;

namespace Devin.DataEncryption.Encryptions
{
    public class SHA1Encryption
    {
        /// <summary>
        /// SHA1 加密
        /// </summary>
        /// <param name="text">加密文本</param>
        /// <param name="uppercase">是否输出大写加密，默认 false</param>
        /// <returns></returns>
        public static string Encrypt(string text, bool uppercase = false)
        {
            return Encrypt(Encoding.UTF8.GetBytes(text), uppercase);
        }

        /// <summary>
        /// MD5 比较
        /// </summary>
        /// <param name="text">加密文本</param>
        /// <param name="hash">SHA1 字符串</param>
        /// <param name="uppercase">是否输出大写加密，默认 false</param>
        /// <returns>bool</returns>
        public static bool Compare(string text, string hash, bool uppercase = false)
        {
            return Compare(Encoding.UTF8.GetBytes(text), hash, uppercase);
        }

        /// <summary>
        /// SHA1 加密
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <param name="uppercase">是否输出大写加密，默认 false</param>
        /// <returns></returns>
        public static string Encrypt(byte[] bytes, bool uppercase = false)
        {
            var data = SHA1.HashData(bytes);

            var stringBuilder = new StringBuilder();
            for (var i = 0; i < data.Length; i++)
            {
                stringBuilder.Append(data[i].ToString("x2"));
            }

            var sha1String = stringBuilder.ToString();
            return !uppercase ? sha1String : sha1String.ToUpper();
        }

        /// <summary>
        /// SHA1 比较
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <param name="hash">SHA1 字符串</param>
        /// <param name="uppercase">是否输出大写加密，默认 false</param>
        /// <returns>bool</returns>
        public static bool Compare(byte[] bytes, string hash, bool uppercase = false)
        {
            var hashOfInput = Encrypt(bytes, uppercase);
            return hash.Equals(hashOfInput, StringComparison.OrdinalIgnoreCase);
        }
    }
}