using System.Collections.Immutable;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core
{
    public interface IKeyValueConfiguration
    {
        ImmutableArray<string> AllKeys { get; }

        ImmutableArray<StringPair> AllValues { get; }

        ImmutableArray<MultipleValuesStringPair> AllWithMultipleValues { get; }

        [CanBeNull] string this[string? key] { get; }
    }
}