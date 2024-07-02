namespace Devin.Options.Attributes
{
    /// <summary>
    /// 配置自动注入配置标注
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class OptionsMetaAttribute : Attribute
    {
        public OptionsMetaAttribute(string key)
        {
            Key = key;
        }

        public string Key { get; set; }
    }
}