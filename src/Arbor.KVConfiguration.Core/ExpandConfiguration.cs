using System;
using System.Collections.Generic;
using System.Collections.Specialized;

using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core
{
    public class ExpandConfiguration : IKeyValueConfiguration
    {
        private readonly IKeyValueConfiguration _inMemoryKeyValueConfiguration;

        public ExpandConfiguration([NotNull] IKeyValueConfiguration keyValueConfiguration)
        {
            if (keyValueConfiguration == null)
            {
                throw new ArgumentNullException(nameof(keyValueConfiguration));
            }

            var collection = new NameValueCollection(keyValueConfiguration.AllKeys.Count);

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
    }
}
