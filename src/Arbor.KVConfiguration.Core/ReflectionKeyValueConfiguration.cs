using System;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Reflection;
using Arbor.KVConfiguration.Core.Metadata;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core
{
    [UsedImplicitly]
    public sealed class ReflectionKeyValueConfiguration : IKeyValueConfigurationWithMetadata
    {
        private readonly string _assemblyName;
        private readonly IKeyValueConfiguration _inMemoryKeyValueConfiguration;

        public ReflectionKeyValueConfiguration([NotNull] Assembly assembly)
        {
            if (assembly is null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            _assemblyName = assembly.FullName;

            ImmutableArray<KeyValueConfigurationItem> keyValueConfigurationItems =
                ReflectionConfigurationReader.ReadConfiguration(assembly);

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

        public ImmutableArray<MultipleValuesStringPair> AllWithMultipleValues =>
            _inMemoryKeyValueConfiguration.AllWithMultipleValues;

        public string this[string key] => _inMemoryKeyValueConfiguration[key];

        public ImmutableArray<KeyValueConfigurationItem> ConfigurationItems { get; }

        public override string ToString() => $"{base.ToString()} [assembly: '{_assemblyName}']";
    }
}