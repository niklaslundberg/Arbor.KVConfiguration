using System.Collections.Immutable;
using System.Linq;
using Arbor.KVConfiguration.Core.Metadata;
using Arbor.KVConfiguration.Schema.Json;

namespace Arbor.KVConfiguration.JsonConfiguration;

internal static class ConfigurationItemsExtensions
{
    internal static ImmutableArray<KeyValueConfigurationItem> ReadConfiguration(
        this ConfigurationItems configurationItems)
    {
        configurationItems.ThrowIfNull(nameof(configurationItems));

        var keyValueConfigurationItems = configurationItems.Keys
            .Select(
                keyValue => new KeyValueConfigurationItem(keyValue.Key,
                    keyValue.Value,
                    keyValue.ConfigurationMetadata))
            .ToImmutableArray();

        return keyValueConfigurationItems;
    }
}