using Devin.TestApi.Options;
using Devin.TestApi.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Devin.TestApi.Controllers
{
    public class TestController : BaseController
    {
        private readonly ILogger<TestController> _logger;
        private readonly ITestService _testService;
        private readonly IOptions<TestOptions> _options;

        public TestController(ILogger<TestController> logger, ITestService testService, IOptions<TestOptions> options)
        {
            _logger = logger;
            _testService = testService;
            _options = options;
        }

        [HttpGet]
        public string Get()
        {
            return "1.0";
        }
    }
}