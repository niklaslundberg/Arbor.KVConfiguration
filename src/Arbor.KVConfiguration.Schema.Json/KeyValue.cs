using System;

namespace Arbor.KVConfiguration.Schema.Json
{
    public class KeyValue
    {
        public string Key { get; }

        public string Value { get; }

        public Metadata Metadata { get; }

        public KeyValue(string key, string value, Metadata metadata)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(key));
            }
            Key = key;
            Value = value;
            Metadata = metadata;
        }

        public bool ShouldSerializeMetadata()
        {
            return Metadata != null;
        }
    }
}
