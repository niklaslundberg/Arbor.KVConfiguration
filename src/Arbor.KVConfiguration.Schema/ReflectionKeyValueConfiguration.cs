using System;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Reflection;
using Arbor.KVConfiguration.Core;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Schema
{
    public class ReflectionKeyValueConfiguration : IKeyValueConfigurationWithMetadata
    {
        private readonly IKeyValueConfiguration _inMemoryKeyValueConfiguration;

        public ReflectionKeyValueConfiguration([NotNull] Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            ImmutableArray<KeyValueConfigurationItem> keyValueConfigurationItems =
                ReflectionConfiguratonReader.ReadConfiguration(assembly);

            var nameValueCollection = new NameValueCollection();

            foreach (KeyValueConfigurationItem keyValueConfigurationItem in keyValueConfigurationItems)
            {
                nameValueCollection.Add(keyValueConfigurationItem.Key, keyValueConfigurationItem.Value);
            }

            _inMemoryKeyValueConfiguration = new InMemoryKeyValueConfiguration(nameValueCollection);
            ConfigurationItems = keyValueConfigurationItems;
        }

        public ImmutableArray<string> AllKeys => _inMemoryKeyValueConfiguration.AllKeys;

        public ImmutableArray<StringPair> AllValues => _inMemoryKeyValueConfiguration.AllValues;

        public ImmutableArray<MultipleValuesStringPair> AllWithMultipleValues
            => _inMemoryKeyValueConfiguration.AllWithMultipleValues;

        public string this[string key] => _inMemoryKeyValueConfiguration[key];

        public ImmutableArray<KeyValueConfigurationItem> ConfigurationItems { get; }
    }
}
