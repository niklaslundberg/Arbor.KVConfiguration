﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core.Metadata.Extensions
{
    public static class KeyValueConfigurationMetadataExtensions
    {
        public static ImmutableArray<KeyValueConfigurationItem> GetKeyValueConfigurationItems(
            [NotNull] this IKeyValueConfigurationWithMetadata keyValueConfiguration)
        {
            if (keyValueConfiguration == null)
            {
                throw new ArgumentNullException(nameof(keyValueConfiguration));
            }

            return keyValueConfiguration.ConfigurationItems;
        }

        public static ImmutableArray<KeyValueConfigurationItem> GetKeyValueConfigurationItems(
            [NotNull] this IKeyValueConfiguration keyValueConfiguration)
        {
            if (keyValueConfiguration == null)
            {
                throw new ArgumentNullException(nameof(keyValueConfiguration));
            }

            if (keyValueConfiguration is IKeyValueConfigurationWithMetadata keyValueConfigurationWithMetadata)
            {
                return keyValueConfigurationWithMetadata.GetKeyValueConfigurationItems();
            }

            ImmutableArray<KeyValueConfigurationItem> keyValueConfigurationItems = keyValueConfiguration
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
}