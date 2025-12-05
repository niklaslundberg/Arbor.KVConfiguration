using System;

namespace Arbor.KVConfiguration.Core.Extensions.LongExtensions;

public static class KeyValueConfigurationLongExtensions
{
    public static long ValueOrDefault(
        this IKeyValueConfiguration keyValueConfiguration,
        string key,
        long defaultValue)
    {
        keyValueConfiguration.ThrowIfNull();

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
        }

        string? value = keyValueConfiguration[key];

        if (!long.TryParse(value, out long parsedResultValue))
        {
            return defaultValue;
        }

        return parsedResultValue;
    }
}