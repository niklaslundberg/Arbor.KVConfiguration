using System;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core.Extensions.BoolExtensions
{
    public static class KeyValueConfigurationBoolExtensions
    {
        public static bool ValueOrDefault(
            [NotNull] this IKeyValueConfiguration keyValueConfiguration,
            [NotNull] string key,
            bool defaultValue = default)
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

            if (!bool.TryParse(value, out bool parsedResultValue))
            {
                return defaultValue;
            }

            return parsedResultValue;
        }
    }
}
