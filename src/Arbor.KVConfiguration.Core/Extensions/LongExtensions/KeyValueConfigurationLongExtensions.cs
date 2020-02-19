using System;

namespace Arbor.KVConfiguration.Core.Extensions.LongExtensions
{
    public static class KeyValueConfigurationLongExtensions
    {
        public static long ValueOrDefault(
            [NotNull] this IKeyValueConfiguration keyValueConfiguration,
            [NotNull] string key,
            long defaultValue)
        {
            if (keyValueConfiguration is null)
            {
                throw new ArgumentNullException(nameof(keyValueConfiguration));
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
            }

            string value = keyValueConfiguration[key];

            if (!long.TryParse(value, out long parsedResultValue))
            {
                return defaultValue;
            }

            return parsedResultValue;
        }
    }
}