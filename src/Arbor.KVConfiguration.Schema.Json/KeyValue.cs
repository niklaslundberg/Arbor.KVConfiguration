using System;

using JetBrains.Annotations;

using Newtonsoft.Json;

namespace Arbor.KVConfiguration.Schema.Json
{
    public class KeyValue
    {
        public KeyValue([NotNull] string key, [CanBeNull] string value, [CanBeNull] Metadata metadata)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(key));
            }

            Key = key;
            Value = value;
            Metadata = metadata;
        }

        [JsonProperty(Order = 0)]
        public string Key { get; }

        [JsonProperty(Order = 2)]
        public Metadata Metadata { get; }

        [JsonProperty(Order = 1)]
        public string Value { get; }

        public bool ShouldSerializeMetadata()
        {
            return Metadata != null;
        }
    }
}