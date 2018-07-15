using System.Collections.Immutable;

namespace Arbor.KVConfiguration.Core
{
    internal class NoConfiguration : IKeyValueConfiguration
    {
        public ImmutableArray<string> AllKeys => ImmutableArray<string>.Empty;

        public ImmutableArray<StringPair> AllValues => ImmutableArray<StringPair>.Empty;

        public ImmutableArray<MultipleValuesStringPair> AllWithMultipleValues =>
            ImmutableArray<MultipleValuesStringPair>.Empty;

        public string this[string key] => "";

        public static readonly IKeyValueConfiguration Empty => new InMemoryConfiguration(new NameValueCollection());
    }
}
