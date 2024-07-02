using Devin.Options.Attributes;
using Devin.Options.Provider;
using Devin.Swagger.Options;
using Devin.TestApi.Options;

namespace Devin.TestApi
{
    public class TestOptionsLoader : BaseOptionsLoader
    {
        public TestOptions TestOptions { get; set; }

        [OptionsMeta("Swagger")]
        public SwaggerOptions SwaggerOptions { get; set; }
    }
}