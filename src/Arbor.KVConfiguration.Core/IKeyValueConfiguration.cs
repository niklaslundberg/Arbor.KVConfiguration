using System.Collections.Generic;

namespace Arbor.KVConfiguration.Core
{
    public interface IKeyValueConfiguration
    {
        IReadOnlyCollection<string> AllKeys { get; }

        IReadOnlyCollection<StringPair> AllValues { get; }

        IReadOnlyCollection<MultipleValuesStringPair> AllWithMultipleValues { get; }

        string this[string key] { get; }

        string ValueOrDefault(string key);

        string ValueOrDefault(string key, string defaultValue);
    }
}