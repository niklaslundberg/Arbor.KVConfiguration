using System.Configuration;
using Arbor.KVConfiguration.Core;

namespace Arbor.KVConfiguration.SystemConfiguration
{
    public sealed class AppSettingsKeyValueConfiguration : IKeyValueConfiguration
    {
        private readonly InMemoryKeyValueConfiguration _inMemoryKeyValueConfiguration;

        public AppSettingsKeyValueConfiguration() => _inMemoryKeyValueConfiguration =
            new InMemoryKeyValueConfiguration(ConfigurationManager.AppSettings);

        public ImmutableArray<string> AllKeys => _inMemoryKeyValueConfiguration.AllKeys;

        public ImmutableArray<StringPair> AllValues => _inMemoryKeyValueConfiguration.AllValues;

        public ImmutableArray<MultipleValuesStringPair> AllWithMultipleValues
            => _inMemoryKeyValueConfiguration.AllWithMultipleValues;

        public string this[string key] => _inMemoryKeyValueConfiguration[key];

        public void Dispose() => _inMemoryKeyValueConfiguration?.Dispose();
    }
}