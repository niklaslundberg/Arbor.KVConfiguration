using System;
using Arbor.KVConfiguration.Core.Metadata;
using Arbor.KVConfiguration.Schema.Json;

namespace Arbor.KVConfiguration.JsonConfiguration
{
    internal static class ConfigurationItemsExtensions
    {
        internal static ImmutableArray<KeyValueConfigurationItem> ReadConfiguration(
            [NotNull] this ConfigurationItems configurationItems)
        {
            if (configurationItems is null)
            {
                throw new ArgumentNullException(nameof(configurationItems));
            }

            var keyValueConfigurationItems = configurationItems.Keys
                .Select(
                    keyValue => new KeyValueConfigurationItem(keyValue.Key,
                        keyValue.Value,
                        keyValue.ConfigurationMetadata))
                .ToImmutableArray();

            return keyValueConfigurationItems;
        }
    }
}