using System;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core.Extensions.LongExtensions
{
    public static class KeyValueConfigurationLongExtensions
    {
        public static long ValueOrDefault(
            [NotNull] this IKeyValueConfiguration keyValueConfiguration,
            [NotNull] string key,
            long defaultValue)
        {
            if (keyValueConfiguration == null)
            {
                throw new ArgumentNullException(nameof(keyValueConfiguration));
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
            }

            string value = keyValueConfiguration[key];

            long parsedResultValue;

            if (!long.TryParse(value, out parsedResultValue))
            {
                return defaultValue;
            }

            return parsedResultValue;
        }
    }
}
