using System;

using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Schema
{
    public struct KeyMetadata
    {
        public KeyMetadata(string key, [CanBeNull] Metadata metadata)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(key));
            }

            Key = key;
            Metadata = metadata;
        }

        public string Key { get; }

        public Metadata Metadata { get; }
    }
}
