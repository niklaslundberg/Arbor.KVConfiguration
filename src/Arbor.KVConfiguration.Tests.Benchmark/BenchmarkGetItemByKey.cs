using System.Collections.Specialized;
using Arbor.KVConfiguration.Core;
using BenchmarkDotNet.Attributes;

namespace Arbor.KVConfiguration.Tests.Benchmark
{
    public class BenchmarkGetItemByKey
    {
        private IKeyValueConfiguration configuration;

        public BenchmarkGetItemByKey()
        {

            var keys = new NameValueCollection
            {
                { "urn:a:complex:immutable:type:instance1:id", "myId1" }
            };

            configuration = new InMemoryKeyValueConfiguration(keys);
        }

        [MemoryDiagnoser]
        [Benchmark]
        public string Value()
        {
            return configuration["urn:a:complex:immutable:type:instance1:id"];
        }
    }
}