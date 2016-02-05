using System;

using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core
{
    public static class KeyValueConfigurationExtensions
    {
        public static string ValueOrDefault(
            [NotNull] this IKeyValueConfiguration keyValueConfiguration,
            string key,
            string defaultValue = "")
        {
            if (keyValueConfiguration == null)
            {
                throw new ArgumentNullException(nameof(keyValueConfiguration));
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                return defaultValue;
            }

            string value = keyValueConfiguration[key];

            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }

            return value;
        }
    }
}