using System;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core.Extensions
{
    public static class BoolExtensions
    {
        public static bool ValueOrDefault([NotNull] this IKeyValueConfiguration keyValueConfiguration, string key, bool defaultValue = default(bool))
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
