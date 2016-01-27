using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Arbor.KVConfiguration.Core
{
    public class ExpandConfiguration : IKeyValueConfiguration
    {
        private readonly IKeyValueConfiguration _inMemoryKeyValueConfiguration;

        public ExpandConfiguration(IKeyValueConfiguration keyValueConfiguration)
        {
            NameValueCollection collection = new NameValueCollection(keyValueConfiguration.AllKeys.Count);

            foreach (MultipleValuesStringPair multipleValuesStringPair in keyValueConfiguration.AllWithMultipleValues)
            {
                foreach (string value in multipleValuesStringPair.Values)
                {
                    string expanded = Environment.ExpandEnvironmentVariables(value);
                    collection.Add(multipleValuesStringPair.Key, expanded);
                }
            }

            _inMemoryKeyValueConfiguration = new InMemoryKeyValueConfiguration(collection);
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