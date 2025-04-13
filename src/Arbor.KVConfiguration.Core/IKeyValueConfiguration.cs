using System.Collections.Immutable;

namespace Arbor.KVConfiguration.Core;

public interface IKeyValueConfiguration
{
    public ImmutableArray<string> AllKeys { get; }

    public ImmutableArray<StringPair> AllValues { get; }

    public ImmutableArray<MultipleValuesStringPair> AllWithMultipleValues { get; }

    public string? this[string? key] { get; }
}