using System.Collections.Immutable;

namespace Arbor.KVConfiguration.Core
{
    public struct NoConfiguration : IKeyValueConfiguration
    {
        public ImmutableArray<string> AllKeys => ImmutableArray<string>.Empty;
        public ImmutableArray<StringPair> AllValues => ImmutableArray<StringPair>.Empty;
        public ImmutableArray<MultipleValuesStringPair> AllWithMultipleValues => ImmutableArray<MultipleValuesStringPair>.Empty;

        public string this[string key] => "";
    }
}