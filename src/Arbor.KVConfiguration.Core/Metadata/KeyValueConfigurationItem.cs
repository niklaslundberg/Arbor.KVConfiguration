using System;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core.Metadata
{
    public struct KeyValueConfigurationItem
    {
        public KeyValueConfigurationItem(
            [NotNull] string key,
            [CanBeNull] string value,
            [CanBeNull] ConfigurationMetadata configurationMetadata)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Value = value;
            ConfigurationMetadata = configurationMetadata;
        }

        public string Key { get; }

        public ConfigurationMetadata ConfigurationMetadata { get; }

        public string Value { get; }

        public override string ToString()
        {
            return $"{nameof(Key)}: {Key}, {nameof(Value)}: {Value}, {nameof(ConfigurationMetadata)}: {ConfigurationMetadata}";
        }
    }
}
