namespace Devin.Extensions.Hangfire.Options
{
    /// <summary>
    /// Hangfire配置
    /// </summary>
    public class HangfireConfig
    {
        /// <summary>
        /// 连接
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Db
        /// </summary>
        public int Db { get; set; }

        /// <summary>
        /// 前缀
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// 队列集合,如果空默认default
        /// </summary>
        public string[] Queues { get; set; }

        /// <summary>
        /// Dashboard账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Dashboard密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 作业超时时间，单位分钟
        /// </summary>
        public int JobExpirationTimeout { get; set; }

        /// <summary>
        /// 是否自动启动
        /// </summary>
        public bool AutoScanAndStart { get; set; } = true;
    }
}