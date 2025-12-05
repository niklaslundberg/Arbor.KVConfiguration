using System;

namespace Arbor.KVConfiguration.Core.Extensions.IntExtensions;

public static class KeyValueConfigurationIntExtensions
{
    public static int ValueOrDefault(
        this IKeyValueConfiguration keyValueConfiguration,
        string key,
        int defaultValue = 0)
    {
        keyValueConfiguration.ThrowIfNull();

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
        }

        string? value = keyValueConfiguration[key];

        if (!int.TryParse(value, out int parsedResultValue))
        {
            return defaultValue;
        }

        return parsedResultValue;
    }
}