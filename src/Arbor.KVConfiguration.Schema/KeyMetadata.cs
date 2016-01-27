using System;

namespace Arbor.KVConfiguration.Schema
{
    public struct KeyMetadata
    {
        public string Key { get; }

        public Metadata Metadata { get; }

        public KeyMetadata(string key, Metadata metadata)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(key));
            }

            Key = key;
            Metadata = metadata;
        }
    }
}