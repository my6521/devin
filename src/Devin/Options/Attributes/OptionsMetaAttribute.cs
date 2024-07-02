namespace Devin.Options.Attributes
{
    /// <summary>
    /// 配置自动注入配置标注
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class OptionsMetaAttribute : Attribute
    {
        public string Key { get; set; }
    }
}