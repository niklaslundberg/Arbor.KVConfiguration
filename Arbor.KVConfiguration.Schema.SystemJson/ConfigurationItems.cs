using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace Arbor.KVConfiguration.Schema.SystemJson;

public class ConfigurationItems(string version, ImmutableArray<KeyValue> keys)
{
    [JsonPropertyOrder(0)]
    [JsonPropertyName(JsonSchemaConstants.VersionPropertyKey)]
    public string Version
    {
        get;
    } = string.IsNullOrWhiteSpace(version)
        ? JsonSchemaConstants.Version1_0
        : version;

    [JsonPropertyOrder(1)] public ImmutableArray<KeyValue> Keys { get; } = keys;
}