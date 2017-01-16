using System;

using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Schema
{
    public struct KeyValueConfigurationItem
    {
        public KeyValueConfigurationItem([NotNull] string key, [CanBeNull] string value, [CanBeNull] ConfigurationMetadata configurationMetadata)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            Key = key;
            Value = value;
            ConfigurationMetadata = configurationMetadata;
        }

        public string Key { get; }

        public ConfigurationMetadata ConfigurationMetadata { get; }

        public string Value { get; }
    }
}
