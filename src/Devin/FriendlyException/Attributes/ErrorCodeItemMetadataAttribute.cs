namespace Devin.FriendlyException.Attributes
{
    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ErrorCodeItemMetadataAttribute : Attribute
    {
        public ErrorCodeItemMetadataAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        public int ErrorCode { get; set; }
    }
}