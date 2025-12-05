using System.Collections.Immutable;

namespace Arbor.KVConfiguration.Core.Decorators;

public interface IKeyValueConfigurationDecorator
{
    public ImmutableArray<string> GetAllKeys(IKeyValueConfiguration keyValueConfiguration);

   public  string? GetValue(string? value);

    public ImmutableArray<StringPair> GetAllValues(IKeyValueConfiguration keyValueConfiguration);

    public ImmutableArray<MultipleValuesStringPair> GetAllWithMultipleValues(
        IKeyValueConfiguration keyValueConfiguration);
}