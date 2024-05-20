namespace Devin.SwaggerDocument.Internal
{
    public sealed class SwaggerLoginInfo
    {
        /// <summary>
        /// 是否启用授权控制
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 检查登录地址
        /// </summary>
        public string CheckUrl { get; set; }

        /// <summary>
        /// 提交登录地址
        /// </summary>
        public string SubmitUrl { get; set; }
    }
}