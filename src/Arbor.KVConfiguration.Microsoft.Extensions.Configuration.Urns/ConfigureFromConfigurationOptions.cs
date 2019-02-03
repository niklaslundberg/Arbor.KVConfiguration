using System;
using System.Collections.Immutable;
using Arbor.KVConfiguration.Urns;
using Microsoft.Extensions.Configuration;

namespace Arbor.KVConfiguration.Microsoft.Extensions.Configuration.Urns
{
    /// <summary>
    /// Configures an option instance by using IKeyValueConfiguration.GetInstance`TOptions against an IConfiguration.
    /// </summary>
    /// <typeparam name="TOptions">The type of options to bind.</typeparam>
    public sealed class ConfigureFromConfigurationOptions<TOptions> : IConfigureConfigurationValue<TOptions>, IDisposable
        where TOptions : class
    {
        private KeyValueConfigurationAdapter _keyValueConfiguration;

        public ConfigureFromConfigurationOptions(IConfiguration configuration)
        {
            _keyValueConfiguration = new KeyValueConfigurationAdapter(configuration);
        }

        public TOptions GetInstance()
        {
            return _keyValueConfiguration.GetInstance<TOptions>();
        }

        public ImmutableArray<TOptions> GetInstances()
        {
            return _keyValueConfiguration.GetInstances<TOptions>();
        }

        public void Dispose()
        {
            _keyValueConfiguration?.Dispose();
            _keyValueConfiguration = null;
        }
    }
}
