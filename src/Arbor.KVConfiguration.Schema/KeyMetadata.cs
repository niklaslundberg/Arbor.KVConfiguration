using System;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Schema
{
    public struct KeyMetadata
    {
        public KeyMetadata(string key, [CanBeNull] ConfigurationMetadata configurationMetadata)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(key));
            }

            Key = key;
            ConfigurationMetadata = configurationMetadata;
        }

        public string Key { get; }

        public ConfigurationMetadata ConfigurationMetadata { get; }
    }
}
