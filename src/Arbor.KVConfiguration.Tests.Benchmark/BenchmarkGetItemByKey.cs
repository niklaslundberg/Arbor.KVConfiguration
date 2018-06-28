using System;
using System.Collections.Specialized;
using Arbor.KVConfiguration.Core;
using BenchmarkDotNet.Attributes;

namespace Arbor.KVConfiguration.Tests.Benchmark
{
    public sealed class BenchmarkGetItemByKey : IDisposable
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

        public void Dispose()
        {
            _configuration?.Dispose();
        }

        [MemoryDiagnoser]
        [Benchmark]
        public string Value()
        {
            return _configuration["urn:a:complex:immutable:type:instance1:id"];
        }
    }
}
