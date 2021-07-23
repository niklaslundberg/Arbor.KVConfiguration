using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core.Metadata
{
    public static class KeyValueConfigurationItemExtensions
    {
        public static ImmutableArray<KeyMetadata> GetMetadata(
            this IEnumerable<KeyValueConfigurationItem>? items)
        {
            if (items is null)
            {
                return ImmutableArray<KeyMetadata>.Empty;
            }

            var keyValueConfigurationItems = items as KeyValueConfigurationItem[]
                                             ?? items.ToArray();

            string[] uniqueKeys = keyValueConfigurationItems.Select(item => item.Key).Distinct().ToArray();

            var ordered = keyValueConfigurationItems.Where(_ => _.ConfigurationMetadata is {})
                .ToArray();

            var readOnlyKeyMetadata =
                uniqueKeys.Select(
                        key => new {key, found = ordered.FirstOrDefault()})
                    .Where(item => item.found?.ConfigurationMetadata is {})
                    .Select(item => new KeyMetadata(item.key, item.found!.ConfigurationMetadata))
                    .ToImmutableArray();

            return readOnlyKeyMetadata;
        }
    }
}