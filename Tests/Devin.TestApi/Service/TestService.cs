using Devin.DependencyInjection.Attributes;
using Devin.DependencyInjection.Dependencies;

namespace Devin.TestApi.Service
{
    [ServiceInjection]
    public class TestService : ITestService, ISingleton
    {
    }
}