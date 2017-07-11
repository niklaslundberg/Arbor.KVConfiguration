using System;

namespace Arbor.KVConfiguration.Core.Extensions
{
    public static class LongExtensions
    {
        public static long ValueOrDefault(
            this IKeyValueConfiguration keyValueConfiguration,
            string key,
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
