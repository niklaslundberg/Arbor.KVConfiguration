namespace Arbor.KVConfiguration.Schema.Json
{
    public class ConfigurationItems
    {
        public ConfigurationItems(string version, ImmutableArray<KeyValue> keys)
        {
            Version = string.IsNullOrWhiteSpace(version)
                ? JsonSchemaConstants.Version1_0
                : version;

            Keys = keys;
        }

        [PublicAPI]
        [JsonProperty(Order = 0, PropertyName = JsonSchemaConstants.VersionPropertyKey)]
        public string Version
        {
            get;
        }

        [JsonProperty(Order = 1)] public ImmutableArray<KeyValue> Keys { get; }
    }
}