using System;
using System.Collections.Immutable;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core
{
    public interface IKeyValueConfiguration : IDisposable
    {
        ImmutableArray<string> AllKeys { get; }

        ImmutableArray<StringPair> AllValues { get; }

        ImmutableArray<MultipleValuesStringPair> AllWithMultipleValues { get; }

        [CanBeNull]
        string this[[CanBeNull] string key] { get; }
    }
}
