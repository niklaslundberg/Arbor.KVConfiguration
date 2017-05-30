using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Linq;
using Arbor.KVConfiguration.Core;
using Microsoft.Extensions.Configuration;

namespace Arbor.KVConfiguration.Microsoft.Extensions.Configuration.Urns
{
    public class KeyValueConfigurationAdapter : IKeyValueConfiguration
    {
        private readonly InMemoryKeyValueConfiguration _inMemoryConfig;

        public KeyValueConfigurationAdapter(IConfiguration config)
        {
            var nameValueCollection = new NameValueCollection();

            foreach (var configurationSection in config.AsEnumerable().Where(pair => !string.IsNullOrWhiteSpace(pair.Key) && !string.IsNullOrWhiteSpace(pair.Value)))
            {
                nameValueCollection.Add(configurationSection.Key, configurationSection.Value);
            }

            _inMemoryConfig = new InMemoryKeyValueConfiguration(nameValueCollection);
        }

        public ImmutableArray<string> AllKeys => _inMemoryConfig.AllKeys;
        public ImmutableArray<StringPair> AllValues => _inMemoryConfig.AllValues;
        public ImmutableArray<MultipleValuesStringPair> AllWithMultipleValues => _inMemoryConfig.AllWithMultipleValues;

        public string this[string key] => _inMemoryConfig[key];
    }
}
