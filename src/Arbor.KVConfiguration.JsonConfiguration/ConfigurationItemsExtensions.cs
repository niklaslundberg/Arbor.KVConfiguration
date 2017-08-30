using System;
using System.Collections.Immutable;
using System.Linq;
using Arbor.KVConfiguration.Core.Metadata;
using Arbor.KVConfiguration.Schema.Json;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.JsonConfiguration
{
    internal static class ConfigurationItemsExtensions
    {
        internal static ImmutableArray<KeyValueConfigurationItem> ReadConfiguration(
            [NotNull] this ConfigurationItems configurationItems)
        {
            if (configurationItems == null)
            {
                throw new ArgumentNullException(nameof(configurationItems));
            }

            ImmutableArray<KeyValueConfigurationItem> keyValueConfigurationItems = configurationItems.Keys
                .Select(
                    keyValue => new KeyValueConfigurationItem(keyValue.Key,
                        keyValue.Value,
                        keyValue.ConfigurationMetadata))
                .ToImmutableArray();

            return keyValueConfigurationItems;
        }
    }
}
