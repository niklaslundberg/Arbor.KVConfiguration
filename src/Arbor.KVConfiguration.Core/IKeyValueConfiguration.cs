using System.Collections.Generic;

using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core
{
    public interface IKeyValueConfiguration
    {
        IReadOnlyCollection<string> AllKeys { get; }

        IReadOnlyCollection<StringPair> AllValues { get; }

        IReadOnlyCollection<MultipleValuesStringPair> AllWithMultipleValues { get; }

        string this[[CanBeNull] string key] { get; }

        string ValueOrDefault([CanBeNull] string key);

        string ValueOrDefault([CanBeNull] string key, [CanBeNull] string defaultValue);
    }
}
