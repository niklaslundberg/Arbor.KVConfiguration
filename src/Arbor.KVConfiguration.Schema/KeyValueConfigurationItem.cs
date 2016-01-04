namespace Arbor.KVConfiguration.Schema
{
    using System;

    using JetBrains.Annotations;

    public class KeyValueConfigurationItem
    {
        public KeyValueConfigurationItem([NotNull] string key, [CanBeNull] string value, [CanBeNull] Metadata metadata)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            Key = key;
            Value = value;
            Metadata = metadata;
        }

        public string Key { get; }

        public Metadata Metadata { get; }

        public string Value { get; }
    }
}