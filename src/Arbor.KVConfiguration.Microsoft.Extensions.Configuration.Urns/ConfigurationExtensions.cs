using Arbor.KVConfiguration.Core;
using Microsoft.Extensions.Configuration;

namespace Arbor.KVConfiguration.Microsoft.Extensions.Configuration.Urns
{
    public static class ConfigurationExtensions
    {
        public static IKeyValueConfiguration ToKeyValueConfigurator(this IConfiguration configuration)
        {
            return new KeyValueConfigurationAdapter(configuration);
        }
    }
}