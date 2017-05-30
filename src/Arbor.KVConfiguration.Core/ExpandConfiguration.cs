using System;
using System.Collections.Immutable;
using System.Collections.Specialized;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core
{
    public sealed class ExpandConfiguration : IKeyValueConfiguration
    {
        private readonly IKeyValueConfiguration _inMemoryKeyValueConfiguration;

        public ExpandConfiguration([NotNull] IKeyValueConfiguration keyValueConfiguration)
        {
            if (keyValueConfiguration == null)
            {
                throw new ArgumentNullException(nameof(keyValueConfiguration));
            }

            var collection = new NameValueCollection(keyValueConfiguration.AllKeys.Length);

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

        public ImmutableArray<string> AllKeys => _inMemoryKeyValueConfiguration.AllKeys;

        public ImmutableArray<StringPair> AllValues => _inMemoryKeyValueConfiguration.AllValues;

        public ImmutableArray<MultipleValuesStringPair> AllWithMultipleValues
            => _inMemoryKeyValueConfiguration.AllWithMultipleValues;

        public string this[string key] => _inMemoryKeyValueConfiguration[key];
    }
}
