namespace Devin.Extensions.Hangfire.Const
{
    /// <summary>
    /// 作业状态常量
    /// </summary>
    public class JobStatusConst
    {
        public const string Created = "Created";
        public const string Enqueued = "Enqueued";
        public const string Scheduled = "Scheduled";
        public const string Processing = " Processing";
        public const string Succeeded = "Succeeded";
        public const string Failed = "Failed";
        public const string Deleted = "Deleted";
    }
}