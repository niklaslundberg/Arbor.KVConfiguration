using System;
using System.Collections;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Linq;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core
{
    [UsedImplicitly]
    public sealed class EnvironmentVariableKeyValueConfigurationSource : IKeyValueConfiguration
    {
        private readonly InMemoryKeyValueConfiguration _inMemoryKeyValueConfiguration;

        public EnvironmentVariableKeyValueConfigurationSource()
        {
            var collection = new NameValueCollection();

            foreach (var item in Environment.GetEnvironmentVariables().OfType<DictionaryEntry>())
            {
                var pair = (item.Key as string ?? item.Key.ToString(),
                    item.Value as string ?? item.Value?.ToString());

                if (!string.IsNullOrWhiteSpace(pair.Item2))
                {
                    collection.Add(pair.Item1, pair.Item2);
                }
            }

            _inMemoryKeyValueConfiguration = new InMemoryKeyValueConfiguration(collection);
        }

        public ImmutableArray<string> AllKeys => _inMemoryKeyValueConfiguration.AllKeys;

        public ImmutableArray<StringPair> AllValues => _inMemoryKeyValueConfiguration.AllValues;

        public ImmutableArray<MultipleValuesStringPair> AllWithMultipleValues
            => _inMemoryKeyValueConfiguration.AllWithMultipleValues;

        public string this[string? key] => _inMemoryKeyValueConfiguration[key];
    }
}