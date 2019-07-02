using System.Collections.Specialized;
using Arbor.KVConfiguration.Core;
using BenchmarkDotNet.Attributes;

namespace Arbor.KVConfiguration.Tests.Benchmark
{
    public class BenchmarkGetItemByKey
    {
        private readonly IKeyValueConfiguration _configuration;

        public BenchmarkGetItemByKey()
        {
            var keys = new NameValueCollection
            {
                { "urn:a:complex:immutable:type:instance1:id", "myId1" }
            };

            _configuration = new InMemoryKeyValueConfiguration(keys);
        }

        [Benchmark]
        public string Value() => _configuration["urn:a:complex:immutable:type:instance1:id"];
    }
}
