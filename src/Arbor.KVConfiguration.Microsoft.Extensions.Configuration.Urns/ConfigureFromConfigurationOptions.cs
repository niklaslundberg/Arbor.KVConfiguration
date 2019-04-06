using System;
using System.Collections.Immutable;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Urns;
using Microsoft.Extensions.Configuration;

namespace Arbor.KVConfiguration.Microsoft.Extensions.Configuration.Urns
{
    /// <summary>
    /// Configures an option instance by using IKeyValueConfiguration.GetInstance`TOptions against an IConfiguration.
    /// </summary>
    /// <typeparam name="TOptions">The type of options to bind.</typeparam>
    public sealed class ConfigureFromConfigurationOptions<TOptions> : IConfigureConfigurationValue<TOptions>, IKeyValueConfiguration, IDisposable
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

        public ImmutableArray<string> AllKeys => _keyValueConfiguration.AllKeys;

        public ImmutableArray<StringPair> AllValues => _keyValueConfiguration.AllValues;

        public ImmutableArray<MultipleValuesStringPair> AllWithMultipleValues => _keyValueConfiguration.AllWithMultipleValues;

        public string this[string key] => _keyValueConfiguration[key];
    }
}
