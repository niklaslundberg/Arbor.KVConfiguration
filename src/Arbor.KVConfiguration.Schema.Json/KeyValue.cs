using System;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Core.Metadata;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Arbor.KVConfiguration.Schema.Json
{
    public class KeyValue
    {
        public KeyValue(
            [NotNull] string key,
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

        [JsonProperty(Order = 0)] public string Key { get; }

        [JsonProperty(Order = 2)] public ConfigurationMetadata? ConfigurationMetadata { get; }

        [JsonProperty(Order = 1)] public string? Value { get; }

        [UsedImplicitly]
        public bool ShouldSerializeConfigurationMetadata() => ConfigurationMetadata is { };
    }
}