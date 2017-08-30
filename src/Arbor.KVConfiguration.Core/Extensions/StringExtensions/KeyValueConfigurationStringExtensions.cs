using System;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core.Extensions.StringExtensions
{
    public static class KeyValueConfigurationStringExtensions
    {
        public static string ValueOrDefault(
            [NotNull] this IKeyValueConfiguration keyValueConfiguration,
            [NotNull] string key,
            [CanBeNull] string defaultValue = "")
        {
            if (keyValueConfiguration == null)
            {
                throw new ArgumentNullException(nameof(keyValueConfiguration));
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
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
