using System.Collections.Immutable;
using Microsoft.Extensions.Configuration;

namespace Arbor.KVConfiguration.Microsoft.Extensions.Configuration.Urns
{
    public static class ConfigurationExtensions
    {
        public static T GetInstance<T>(this IConfiguration configuration) where T : class
        {
            var configureFromConfigurationOptions = new ConfigureFromConfigurationOptions<T>(configuration);

            T instance = configureFromConfigurationOptions.GetInstance();

            return instance;
        }

        public static ImmutableArray<T> GetInstances<T>(this IConfiguration configuration) where T : class
        {
            var configureFromConfigurationOptions = new ConfigureFromConfigurationOptions<T>(configuration);

            ImmutableArray<T> instances = configureFromConfigurationOptions.GetInstances();

            return instances;
        }
    }
}
