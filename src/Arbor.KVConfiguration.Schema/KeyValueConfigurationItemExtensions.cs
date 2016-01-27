using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Arbor.KVConfiguration.Schema
{
    public static class KeyValueConfigurationItemExtensions
    {
        public static IReadOnlyCollection<KeyMetadata> GetMetadata(this IEnumerable<KeyValueConfigurationItem> items)
        {
            if (items == null)
            {
                return new KeyMetadata[] { };
            }

            var keyValueConfigurationItems = items as KeyValueConfigurationItem[] ?? items.ToArray();

            var uniqueKeys = keyValueConfigurationItems.Select(item => item.Key).Distinct();

            var ordered = keyValueConfigurationItems.Where(_ => _.Metadata != null).ToArray();

            var list =
                uniqueKeys.Select(key => new { key, found = ordered.FirstOrDefault() })
                    .Select(@t => new KeyMetadata(@t.key, @t.found.Metadata))
                    .ToList();

            return new ReadOnlyCollection<KeyMetadata>(list);
        }
    }
}