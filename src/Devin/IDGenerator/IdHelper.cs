namespace Devin.IDGenerator
{
    /// <summary>
    /// 这是一个调用的例子，默认情况下，单机集成者可以直接使用 NextId()。
    /// </summary>
    public class IdHelper
    {
        private static IIdGenerator _IdGenInstance = null;

        public static IIdGenerator IdGenInstance => _IdGenInstance;

        /// <summary>
        /// 设置参数，建议程序初始化时执行一次
        /// </summary>
        /// <param name="options"></param>
        public static void SetIdGenerator(IdGeneratorOptions options)
        {
            _IdGenInstance = new DefaultIdGenerator(options);
        }

        /// <summary>
        /// 生成新的Id
        /// 调用本方法前，请确保调用了 SetIdGenerator 方法做初始化。
        /// 否则将会初始化一个WorkerId为1的对象。
        /// </summary>
        /// <returns></returns>
        public static long NextId()
        {
            if (_IdGenInstance == null)
            {
                _IdGenInstance = new DefaultIdGenerator(
                    new IdGeneratorOptions() { WorkerId = 0 }
                    );
            }

            return _IdGenInstance.NewLong();
        }
    }
}