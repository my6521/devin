using Devin.FriendlyException.Attributes;
using Devin.Utitlies;
using System.Collections.Concurrent;
using System.Reflection;

namespace Devin.FriendlyException
{
    /// <summary>
    /// 抛异常静态类
    /// </summary>
    public static class Oops
    {
        /// <summary>
        /// 错误代码类型
        /// </summary>
        private static readonly IEnumerable<Type> _errorCodeTypes;

        /// <summary>
        /// 错误消息字典
        /// </summary>
        private static readonly ConcurrentDictionary<string, ErrorCodeItemMetadataAttribute> _errorCodeMessages;

        static Oops()
        {
            _errorCodeTypes = GetErrorCodeTypes();
            _errorCodeMessages = GetErrorCodeMessages();
        }

        public static AppFriendlyException Oh(string message, params string[] formatTexts)
        {
            if (formatTexts.Length > 0)
            {
                message = string.Format(message, formatTexts);
            }

            return new AppFriendlyException(message);
        }

        public static AppFriendlyException Oh(string message, int errorCode, params string[] formatTexts)
        {
            if (formatTexts.Length > 0)
            {
                message = string.Format(message, formatTexts);
            }

            return new AppFriendlyException(message, errorCode);
        }

        public static AppFriendlyException Oh(Enum errorCode, params string[] formatTexts)
        {
            var key = $"{errorCode.GetType().FullName}::{errorCode.ToString()}";
            var errorCodeMessage = _errorCodeMessages[key];
            var message = errorCodeMessage.ErrorMessage;
            if (formatTexts.Length > 0)
            {
                message = string.Format(message, formatTexts);
            }
            return new AppFriendlyException(message, errorCodeMessage.ErrorCode);
        }

        #region 私有方法

        private static IEnumerable<Type> GetErrorCodeTypes()
        {
            var errorCodeTypes = RuntimeUtil.AllAssemblies.SelectMany(x => x.ExportedTypes.Where(u => u.IsDefined(typeof(ErrorCodeTypeAttribute), true) && u.IsEnum));

            return errorCodeTypes.Distinct();
        }

        private static ConcurrentDictionary<string, ErrorCodeItemMetadataAttribute> GetErrorCodeMessages()
        {
            var defaultErrorCodeMessages = new ConcurrentDictionary<string, ErrorCodeItemMetadataAttribute>();

            var errorCodeMessages = _errorCodeTypes.SelectMany(u => u.GetFields()
                .Where(u => u.IsDefined(typeof(ErrorCodeItemMetadataAttribute))))
                .Select(u => GetErrorCodeItemInformation(u))
               .ToDictionary(u => u.Key, u => u.value);

            foreach (var (key, value) in errorCodeMessages)
            {
                defaultErrorCodeMessages.AddOrUpdate(key, value, (key, old) => value);
            }

            return defaultErrorCodeMessages;
        }

        private static (string Key, ErrorCodeItemMetadataAttribute value) GetErrorCodeItemInformation(FieldInfo fieldInfo)
        {
            var key = $"{fieldInfo.FieldType.FullName}::{fieldInfo.Name}";
            var errorCodeItemMetadata = fieldInfo.GetCustomAttribute<ErrorCodeItemMetadataAttribute>();
            return (key, errorCodeItemMetadata);
        }

        #endregion 私有方法
    }
}