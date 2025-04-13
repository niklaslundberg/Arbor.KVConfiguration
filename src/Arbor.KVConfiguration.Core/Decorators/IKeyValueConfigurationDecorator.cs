using System.Collections.Immutable;

namespace Arbor.KVConfiguration.Core.Decorators;

public interface IKeyValueConfigurationDecorator
{
    ImmutableArray<string> GetAllKeys(IKeyValueConfiguration keyValueConfiguration);

    string GetValue(string value);

    ImmutableArray<StringPair> GetAllValues(IKeyValueConfiguration keyValueConfiguration);

    ImmutableArray<MultipleValuesStringPair> GetAllWithMultipleValues(
        IKeyValueConfiguration keyValueConfiguration);
}