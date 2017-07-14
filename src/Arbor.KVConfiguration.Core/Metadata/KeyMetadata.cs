using System;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core.Metadata
{
    public struct KeyMetadata
    {
        public KeyMetadata([NotNull] string key, [CanBeNull] ConfigurationMetadata configurationMetadata)
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
