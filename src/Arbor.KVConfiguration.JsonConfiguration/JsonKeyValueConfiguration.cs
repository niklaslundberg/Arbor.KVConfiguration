using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.IO;

using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Schema;

using JetBrains.Annotations;

namespace Arbor.KVConfiguration.JsonConfiguration
{
    public class JsonKeyValueConfiguration : IKeyValueConfiguration
    {
        private readonly IKeyValueConfiguration _inMemoryKeyValueConfiguration;

        public JsonKeyValueConfiguration([NotNull] IEnumerable<KeyValueConfigurationItem> keyValueConfigurationItems)
        {
            if (keyValueConfigurationItems == null)
            {
                throw new ArgumentNullException(nameof(keyValueConfigurationItems));
            }

            var nameValueCollection = new NameValueCollection();

            foreach (KeyValueConfigurationItem keyValueConfigurationItem in keyValueConfigurationItems)
            {
                nameValueCollection.Add(keyValueConfigurationItem.Key, keyValueConfigurationItem.Value);
            }

            _inMemoryKeyValueConfiguration = new InMemoryKeyValueConfiguration(nameValueCollection);
        }

        public JsonKeyValueConfiguration(string fileFullPath)
            : this(ReadJsonFile(fileFullPath))
        {
        }

        public ImmutableArray<string> AllKeys => _inMemoryKeyValueConfiguration.AllKeys;

        public ImmutableArray<StringPair> AllValues => _inMemoryKeyValueConfiguration.AllValues;

        public ImmutableArray<MultipleValuesStringPair> AllWithMultipleValues
            => _inMemoryKeyValueConfiguration.AllWithMultipleValues;

        public string this[string key] => _inMemoryKeyValueConfiguration[key];

        private static ImmutableArray<KeyValueConfigurationItem> ReadJsonFile(string fileFullPath)
        {
            if (string.IsNullOrWhiteSpace(fileFullPath))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(fileFullPath));
            }

            if (!File.Exists(fileFullPath))
            {
                throw new ArgumentException($"The file '{fileFullPath}' does not exist", nameof(fileFullPath));
            }

            var jsonFileReader = new JsonFileReader(fileFullPath);

            ImmutableArray<KeyValueConfigurationItem> keyValueConfigurationItems =
                jsonFileReader.ReadConfiguration();

            return keyValueConfigurationItems;
        }
    }
}
