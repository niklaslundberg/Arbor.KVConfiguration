using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using Newtonsoft.Json;
using System.Collections.Immutable;

namespace Arbor.KVConfiguration.Schema.Json
{
    public class ConfigurationItems
    {
        public ConfigurationItems(string version, ImmutableArray<KeyValue> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            Version = string.IsNullOrWhiteSpace(version) ? JsonConstants.Version1_0 : version;
            Keys = keys;
        }

        [JsonProperty(Order = 1)]
        public ImmutableArray<KeyValue> Keys { get; }

        [JsonProperty(Order = 0, PropertyName = JsonConstants.VersionPropertyKey)]
        public string Version { [UsedImplicitly] get; }
    }
}
