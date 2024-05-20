namespace Devin.JwtBearer
{
    /// <summary>
    /// token实体类
    /// </summary>
    public class TokenResult
    {
        /// <summary>
        /// Token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>
        public int Expire { get; set; }
    }
}