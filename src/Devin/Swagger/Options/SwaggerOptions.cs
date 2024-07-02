using Devin.Options;
using Devin.Swagger.Internal;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Devin.Swagger.Options
{
    public class SwaggerOptions : IOptionsInit
    {
        public string DocumentTitle { get; set; }
        public DocExpansion DocExpansionState { get; set; }
        public SwaggerLoginInfo LoginInfo { get; set; }
        public string DefaultGroupName { get; set; }
        public string AllGroupName { get; set; }
        public List<SwaggerGroupInfo> ApiGroups { get; set; }
    }
}