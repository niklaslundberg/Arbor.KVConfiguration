using System;

namespace Arbor.KVConfiguration.Core.Extensions.StringExtensions;

public static class KeyValueConfigurationStringExtensions
{
    public static string? ValueOrDefault(
        this IKeyValueConfiguration keyValueConfiguration,
        string key,
        string? defaultValue = "")
    {
        keyValueConfiguration.ThrowIfNull();

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        string? value = keyValueConfiguration[key];

        if (string.IsNullOrWhiteSpace(value))
        {
            return defaultValue;
        }

        return value;
    }
}