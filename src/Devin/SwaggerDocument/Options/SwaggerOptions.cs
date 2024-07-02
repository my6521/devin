using Devin.Options.Attributes;
using Devin.SwaggerDocument.Internal;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Devin.SwaggerDocument.Options
{
    [IgnoreOptionInjection]
    public class SwaggerOptions
    {
        public string DocumentTitle { get; set; }
        public DocExpansion DocExpansionState { get; set; }
        public SwaggerLoginInfo LoginInfo { get; set; }
        public string DefaultGroupName { get; set; }
        public string AllGroupName { get; set; }
        public List<SwaggerGroupInfo> ApiGroups { get; set; }
    }
}