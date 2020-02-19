using System.Collections.Generic;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Microsoft.Extensions.Configuration.Urns;
using Arbor.KVConfiguration.Urns;

namespace Arbor.KVConfiguration.Tests.Integration
{
    public class WhenUsingMicrosoftConfigurationAsSourceInIKeyValueConfiguration
    {
        [Fact]
        public void ItShouldUseValuesDefined()
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {["a:b:c"] = "123"}).Build();

            var multiSourceKeyValueConfiguration = KeyValueConfigurationManager
                .Add(new KeyValueConfigurationAdapter(configurationRoot)).Build();

            string actual = multiSourceKeyValueConfiguration["a:b:c"];

            Assert.Equal("123", actual);
        }

        [Fact]
        public void ItShouldResolveSingleInstanceFromUrn()
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["urn:test:simple:instance:name"] = "John", ["urn:test:simple:instance:age"] = "42"
                }).Build();

            var multiSourceKeyValueConfiguration = KeyValueConfigurationManager
                .Add(new KeyValueConfigurationAdapter(configurationRoot)).Build();

            var actual = multiSourceKeyValueConfiguration.GetInstance<SimpleCtorType>();

            Assert.Equal(new SimpleCtorType("John", 42), actual, SimpleCtorType.NameAgeComparer);
        }
    }
}