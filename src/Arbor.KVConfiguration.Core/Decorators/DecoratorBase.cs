using System.Collections.Immutable;
using System.Linq;
using Arbor.KVConfiguration.Core.Extensions;

namespace Arbor.KVConfiguration.Core.Decorators;

public abstract class DecoratorBase : IKeyValueConfigurationDecorator
{
    public ImmutableArray<string> GetAllKeys(IKeyValueConfiguration keyValueConfiguration)
    {
       keyValueConfiguration.ThrowIfNull();

        return keyValueConfiguration.AllKeys;
    }

    public abstract string? GetValue(string? value);

    public ImmutableArray<StringPair> GetAllValues(IKeyValueConfiguration keyValueConfiguration)
    {
        keyValueConfiguration.ThrowIfNull();

        return [..keyValueConfiguration.AllValues.Select(item => new StringPair(item.Key, GetValue(item.Value)))];
    }

    public ImmutableArray<MultipleValuesStringPair> GetAllWithMultipleValues(
        IKeyValueConfiguration keyValueConfiguration)
    {
        keyValueConfiguration.ThrowIfNull();

        return [
            ..keyValueConfiguration.AllWithMultipleValues.Select(item => new MultipleValuesStringPair(item.Key,
                [..item.Values.Select(GetValue)]))
        ];
    }
}