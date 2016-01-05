using System.Collections.Generic;
using System.Configuration;

using Arbor.KVConfiguration.Core;

namespace Arbor.KVConfiguration.SystemConfiguration
{
    public class AppSettingsKeyValueConfiguration : IKeyValueConfiguration
    {
        private readonly InMemoryKeyValueConfiguration _inMemoryKeyValueConfiguration;

        public AppSettingsKeyValueConfiguration()
        {
            _inMemoryKeyValueConfiguration = new InMemoryKeyValueConfiguration(ConfigurationManager.AppSettings);
        }

        public IReadOnlyCollection<string> AllKeys => _inMemoryKeyValueConfiguration.AllKeys;

        public IReadOnlyCollection<StringPair> AllValues => _inMemoryKeyValueConfiguration.AllValues;

        public IReadOnlyCollection<MultipleValuesStringPair> AllWithMultipleValues
            => _inMemoryKeyValueConfiguration.AllWithMultipleValues;

        public string this[string key] => _inMemoryKeyValueConfiguration[key];

        public string ValueOrDefault(string key)
        {
            return _inMemoryKeyValueConfiguration.ValueOrDefault(key);
        }

        public string ValueOrDefault(string key, string defaultValue)
        {
            return _inMemoryKeyValueConfiguration.ValueOrDefault(defaultValue);
        }
    }
}