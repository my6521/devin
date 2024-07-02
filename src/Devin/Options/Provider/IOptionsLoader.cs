using Microsoft.Extensions.Configuration;

namespace Devin.Options.Provider
{
    public interface IOptionsLoader
    {
        void Load(IConfiguration configuration, IDictionary<Type, object> map);
    }
}