using System.Text.Json.Serialization;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Core.Metadata;

namespace Arbor.KVConfiguration.Schema.SystemJson;

public class KeyValue
{
    public KeyValue(
        string key,
        string? value,
        ConfigurationMetadata? configurationMetadata)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException(KeyValueResources.ArgumentIsNullOrWhitespace, nameof(key));
        }

        Key = key;
        Value = value;
        ConfigurationMetadata = configurationMetadata;
    }

    [JsonPropertyOrder(0)] public string Key { get; }

    [JsonPropertyOrder(2)] public ConfigurationMetadata? ConfigurationMetadata { get; }

    [JsonPropertyOrder(1)] public string? Value { get; }

    
    public bool ShouldSerializeConfigurationMetadata() => ConfigurationMetadata is { };
}