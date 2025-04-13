using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Arbor.KVConfiguration.Core.Extensions;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core.Metadata.Extensions;

public static class KeyValueConfigurationMetadataExtensions
{
    [PublicAPI]
    public static ImmutableArray<KeyValueConfigurationItem> GetKeyValueConfigurationItems(
        this IKeyValueConfigurationWithMetadata keyValueConfiguration)
    {
        keyValueConfiguration.ThrowIfNull();

        return keyValueConfiguration.ConfigurationItems;
    }

    public static ImmutableArray<KeyValueConfigurationItem> GetKeyValueConfigurationItems(
        this IKeyValueConfiguration keyValueConfiguration)
    {
        keyValueConfiguration.ThrowIfNull();

        if (keyValueConfiguration is IKeyValueConfigurationWithMetadata keyValueConfigurationWithMetadata)
        {
            return keyValueConfigurationWithMetadata.GetKeyValueConfigurationItems();
        }

        var keyValueConfigurationItems = keyValueConfiguration
            .AllWithMultipleValues.Select(item =>
            {
                var configurationItems = new List<KeyValueConfigurationItem>();

                foreach (string value in item.Values)
                {
                    configurationItems.Add(
                        new KeyValueConfigurationItem(item.Key, value, null));
                }

                return configurationItems;
            })
            .SelectMany(_ => _)
            .ToImmutableArray();

        return keyValueConfigurationItems;
    }
}