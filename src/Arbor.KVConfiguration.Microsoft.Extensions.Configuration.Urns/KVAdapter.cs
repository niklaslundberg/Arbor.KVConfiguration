using System;
using System.Collections.Immutable;
using Arbor.KVConfiguration.Core;
using Microsoft.Extensions.Configuration;

namespace Arbor.KVConfiguration.Microsoft.Extensions.Configuration.Urns
{
    public class KVAdapter : IKeyValueConfiguration
    {
        public KVAdapter(IConfiguration config)
        {
            throw new NotImplementedException();
        }

        public ImmutableArray<string> AllKeys { get; }
        public ImmutableArray<StringPair> AllValues { get; }
        public ImmutableArray<MultipleValuesStringPair> AllWithMultipleValues { get; }

        public string this[string key]
        {
            get { throw new NotImplementedException(); }
        }
    }
}