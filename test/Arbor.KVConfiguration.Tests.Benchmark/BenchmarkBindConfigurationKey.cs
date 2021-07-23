using System.Collections.Specialized;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.DependencyInjection;
using Arbor.KVConfiguration.Urns;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Arbor.KVConfiguration.Tests.Benchmark
{
    public class BenchmarkBindConfigurationKey
    {
        private readonly ServiceProvider _serviceProvider;

        public BenchmarkBindConfigurationKey()
        {
            var keys = new NameValueCollection
            {
                {"urn:required:type:with:validation:instance:name", "abc"},
                {"urn:required:type:with:validation:instance:value", "123"}
            };

            var configuration = new InMemoryKeyValueConfiguration(keys);

            var configurationRegistrations =
                configuration.GetRegistrations(typeof(ValidatableRequired));

            var configurationInstanceHolder = configurationRegistrations.CreateHolder();

            _serviceProvider = new ServiceCollection()
                .AddConfigurationInstanceHolder(configurationInstanceHolder)
                .BuildServiceProvider();
        }

        [Benchmark]
        public ValidatableRequired Value() => _serviceProvider.GetRequiredService<ValidatableRequired>();
    }
}