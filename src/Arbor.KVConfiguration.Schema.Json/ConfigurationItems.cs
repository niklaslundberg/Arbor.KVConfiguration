using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Arbor.KVConfiguration.Schema.Json
{
    public class ConfigurationItems
    {
        public ConfigurationItems(string version, IReadOnlyCollection<KeyValue> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            if (string.IsNullOrWhiteSpace(version))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(version));
            }

            Version = version;
            Keys = keys;
        }

        [JsonProperty(Order = 1)]
        public IReadOnlyCollection<KeyValue> Keys { get; }

        [JsonProperty(Order = 0)]
        public string Version { get; }
    }
}
