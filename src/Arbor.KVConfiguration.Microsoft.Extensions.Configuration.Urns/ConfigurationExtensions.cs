using Arbor.KVConfiguration.Core;

namespace Arbor.KVConfiguration.Microsoft.Extensions.Configuration.Urns
{
    public static class ConfigurationExtensions
    {
        public static IKeyValueConfiguration ToKeyValueConfigurator(this IConfiguration configuration) =>
            new KeyValueConfigurationAdapter(configuration);
    }
}