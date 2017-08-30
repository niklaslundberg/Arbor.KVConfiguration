using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core.Metadata
{
    public static class KeyValueConfigurationItemExtensions
    {
        public static ImmutableArray<KeyMetadata> GetMetadata(
            [CanBeNull] this IEnumerable<KeyValueConfigurationItem> items)
        {
            if (items == null)
            {
                return ImmutableArray<KeyMetadata>.Empty;
            }

            KeyValueConfigurationItem[] keyValueConfigurationItems = items as KeyValueConfigurationItem[]
                                                                     ?? items.ToArray();

            string[] uniqueKeys = keyValueConfigurationItems.Select(item => item.Key).Distinct().ToArray();

            KeyValueConfigurationItem[] ordered = keyValueConfigurationItems.Where(_ => _.ConfigurationMetadata != null)
                .ToArray();

            ImmutableArray<KeyMetadata> readOnlyKeyMetadata =
                uniqueKeys.Select(
                        key => new
                        {
                            key,
                            found = ordered.FirstOrDefault()
                        })
                    .Select(item => new KeyMetadata(item.key, item.found.ConfigurationMetadata))
                    .ToImmutableArray();

            return readOnlyKeyMetadata;
        }
    }
}
