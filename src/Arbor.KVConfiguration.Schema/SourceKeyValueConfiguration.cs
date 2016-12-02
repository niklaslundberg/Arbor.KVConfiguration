using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using Arbor.KVConfiguration.Core;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Schema
{
    public class SourceKeyValueConfiguration : IKeyValueConfiguration
    {
        private readonly IKeyValueConfiguration _inMemoryKeyValueConfiguration;

        public SourceKeyValueConfiguration([NotNull] Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            IReadOnlyCollection<KeyValueConfigurationItem> keyValueConfigurationItems = new SourceReader().ReadConfiguration(assembly);

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
    }
}
