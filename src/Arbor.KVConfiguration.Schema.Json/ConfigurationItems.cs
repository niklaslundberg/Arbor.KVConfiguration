using System.Collections.Immutable;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Arbor.KVConfiguration.Schema.Json
{
    public class ConfigurationItems
    {
        public ConfigurationItems(string version, ImmutableArray<KeyValue> keys)
        {
            Version = string.IsNullOrWhiteSpace(version) ? JsonSchemaConstants.Version1_0 : version;
            Keys = keys;
        }

        [JsonProperty(Order = 0, PropertyName = JsonSchemaConstants.VersionPropertyKey)]
        public string Version
        {
            [UsedImplicitly]
            get;
        }

        [JsonProperty(Order = 1)]
        public ImmutableArray<KeyValue> Keys { get; }
    }
}
