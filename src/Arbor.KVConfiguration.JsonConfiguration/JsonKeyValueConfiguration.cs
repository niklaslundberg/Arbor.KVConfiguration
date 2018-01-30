using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.IO;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Core.Metadata;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.JsonConfiguration
{
    public sealed class JsonKeyValueConfiguration : IKeyValueConfigurationWithMetadata
    {
        private readonly IKeyValueConfiguration _inMemoryKeyValueConfiguration;
        private string _fileFullPath;

        public JsonKeyValueConfiguration([NotNull] IEnumerable<KeyValueConfigurationItem> keyValueConfigurationItems)
        {
            if (keyValueConfigurationItems == null)
            {
                throw new ArgumentNullException(nameof(keyValueConfigurationItems));
            }

            ImmutableArray<KeyValueConfigurationItem> items = keyValueConfigurationItems.SafeToImmutableArray();

            var nameValueCollection = new NameValueCollection();

            foreach (KeyValueConfigurationItem keyValueConfigurationItem in items)
            {
                nameValueCollection.Add(keyValueConfigurationItem.Key, keyValueConfigurationItem.Value);
            }

            _inMemoryKeyValueConfiguration = new InMemoryKeyValueConfiguration(nameValueCollection);

            ConfigurationItems = items;
        }

        public JsonKeyValueConfiguration(string fileFullPath, bool throwWhenNotExists = true)
            : this(ReadJsonFile(fileFullPath, throwWhenNotExists))
        {
            _fileFullPath = fileFullPath;
        }

        public ImmutableArray<string> AllKeys => _inMemoryKeyValueConfiguration.AllKeys;

        public ImmutableArray<StringPair> AllValues => _inMemoryKeyValueConfiguration.AllValues;

        public ImmutableArray<MultipleValuesStringPair> AllWithMultipleValues
            => _inMemoryKeyValueConfiguration.AllWithMultipleValues;

        public string this[string key] => _inMemoryKeyValueConfiguration[key];

        public ImmutableArray<KeyValueConfigurationItem> ConfigurationItems { get; }

        private static ImmutableArray<KeyValueConfigurationItem> ReadJsonFile(
            string fileFullPath,
            bool throwWhenNotExists)
        {
            if (string.IsNullOrWhiteSpace(fileFullPath))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(fileFullPath));
            }

            if (!File.Exists(fileFullPath))
            {
                if (throwWhenNotExists)
                {
                    throw new ArgumentException($"The file '{fileFullPath}' does not exist", nameof(fileFullPath));
                }

                return ImmutableArray<KeyValueConfigurationItem>.Empty;
            }

            var jsonFileReader = new JsonFileReader(fileFullPath);

            ImmutableArray<KeyValueConfigurationItem> keyValueConfigurationItems =
                jsonFileReader.ReadConfiguration();

            return keyValueConfigurationItems;
        }

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(_fileFullPath))
            {
                return $"{base.ToString()} [json file source: '{_fileFullPath}', exists: {File.Exists(_fileFullPath)}]";
            }

            return $"{base.ToString()} [no json file source]";
        }

        public void Dispose()
        {
            _inMemoryKeyValueConfiguration?.Dispose();
        }
    }
}
