using System.Collections.Generic;
using System.Collections.Specialized;

using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Schema;

namespace Arbor.KVConfiguration.JsonConfiguration
{
    public class JsonKeyValueConfiguration : IKeyValueConfiguration
    {
        private readonly IKeyValueConfiguration _inMemoryKeyValueConfiguration;

        public JsonKeyValueConfiguration(string fileFullPath)
        {
            var jsonFileReader = new JsonFileReader(fileFullPath);
            IReadOnlyCollection<KeyValueConfigurationItem> keyValueConfigurationItems =
                jsonFileReader.ReadConfiguration();

            var nameValueCollection = new NameValueCollection();

            foreach (KeyValueConfigurationItem keyValueConfigurationItem in keyValueConfigurationItems)
            {
                nameValueCollection.Add(keyValueConfigurationItem.Key, keyValueConfigurationItem.Value);
            }

            _inMemoryKeyValueConfiguration = new InMemoryKeyValueConfiguration(nameValueCollection);
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