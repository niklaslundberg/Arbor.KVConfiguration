using System.Collections.Immutable;
using System.Collections.Specialized;

namespace Arbor.KVConfiguration.Core
{
    public class NoConfiguration : IKeyValueConfiguration
    {
        public ImmutableArray<string> AllKeys => ImmutableArray<string>.Empty;

        public ImmutableArray<StringPair> AllValues => ImmutableArray<StringPair>.Empty;

        public ImmutableArray<MultipleValuesStringPair> AllWithMultipleValues =>
            ImmutableArray<MultipleValuesStringPair>.Empty;

        public string this[string key] => string.Empty;

        public static readonly IKeyValueConfiguration Empty = new InMemoryKeyValueConfiguration(new NameValueCollection());
    }
}
