using System;
using System.Collections.Immutable;
using System.Linq;
using Arbor.KVConfiguration.Core.Extensions;

namespace Arbor.KVConfiguration.Core.Decorators;

public sealed class ExpandKeyValueConfigurationDecorator : IKeyValueConfigurationDecorator
{
    public ImmutableArray<string> GetAllKeys(IKeyValueConfiguration keyValueConfiguration)
    {
        keyValueConfiguration.ThrowIfNull();

        return keyValueConfiguration.AllKeys;
    }

    public string GetValue(string value) => ExpandValue(value);

    public ImmutableArray<StringPair> GetAllValues(IKeyValueConfiguration keyValueConfiguration)
    {
        keyValueConfiguration.ThrowIfNull();

        return [.. keyValueConfiguration.AllValues.Select(item => new StringPair(item.Key, ExpandValue(item.Value)))];
    }

    public ImmutableArray<MultipleValuesStringPair> GetAllWithMultipleValues(
        IKeyValueConfiguration keyValueConfiguration)
    {
        keyValueConfiguration.ThrowIfNull();

        return [
            ..keyValueConfiguration.AllWithMultipleValues.Select(item => new MultipleValuesStringPair(item.Key,
                [..item.Values.Select(ExpandValue)]))
        ];
    }

    private static string ExpandValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        string expanded = Environment.ExpandEnvironmentVariables(value);

        return expanded;
    }
}