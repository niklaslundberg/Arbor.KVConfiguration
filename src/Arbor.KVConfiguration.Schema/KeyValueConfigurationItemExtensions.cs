using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Schema
{
    public static class KeyValueConfigurationItemExtensions
    {
        [NotNull]
        public static IReadOnlyCollection<KeyMetadata> GetMetadata(
            [CanBeNull] this IEnumerable<KeyValueConfigurationItem> items)
        {
            if (items == null)
            {
                return ArrayExtensions<KeyMetadata>.Empty();
            }

            KeyValueConfigurationItem[] keyValueConfigurationItems = items as KeyValueConfigurationItem[]
                                                                     ?? items.ToArray();

            string[] uniqueKeys = keyValueConfigurationItems.Select(item => item.Key).Distinct().ToArray();

            KeyValueConfigurationItem[] ordered = keyValueConfigurationItems.Where(_ => _.Metadata != null).ToArray();

            List<KeyMetadata> list =
                uniqueKeys.Select(
                    key => new
                               {
                                   key,
                                   found = ordered.FirstOrDefault()
                               })
                    .Select(item => new KeyMetadata(item.key, item.found.Metadata))
                    .ToList();

            var readOnlyKeyMetadata = new ReadOnlyCollection<KeyMetadata>(list);

            return readOnlyKeyMetadata;
        }
    }
}
