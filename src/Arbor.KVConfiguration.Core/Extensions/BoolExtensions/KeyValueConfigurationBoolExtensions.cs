using System;

namespace Arbor.KVConfiguration.Core.Extensions.BoolExtensions;

public static class KeyValueConfigurationBoolExtensions
{
    public static bool ValueOrDefault(
        this IKeyValueConfiguration keyValueConfiguration,
        string key,
        bool defaultValue = false)
    {
        keyValueConfiguration.ThrowIfNull();

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
        }

        string? value = keyValueConfiguration[key];

        if (!bool.TryParse(value, out bool parsedResultValue))
        {
            return defaultValue;
        }

        return parsedResultValue;
    }
}