using System;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core.Extensions.IntExtensions
{
    public static class KeyValueConfigurationIntExtensions
    {
        public static int ValueOrDefault(
            [NotNull] this IKeyValueConfiguration keyValueConfiguration,
            [NotNull] string key,
            int defaultValue = default)
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

            if (!int.TryParse(value, out int parsedResultValue))
            {
                return defaultValue;
            }

            return parsedResultValue;
        }
    }
}
