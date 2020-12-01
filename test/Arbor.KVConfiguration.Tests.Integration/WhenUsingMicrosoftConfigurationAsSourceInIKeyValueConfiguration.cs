using System.Collections.Generic;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Microsoft.Extensions.Configuration.Urns;
using Arbor.KVConfiguration.Urns;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Arbor.KVConfiguration.Tests.Integration
{
    public class WhenUsingMicrosoftConfigurationAsSourceInIKeyValueConfiguration
    {
        [Fact]
        public void ItShouldResolveSingleInstanceFromUrn()
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["urn:test:simple:instance:name"] = "John", ["urn:test:simple:instance:age"] = "42"
                }).Build();

            MultiSourceKeyValueConfiguration multiSourceKeyValueConfiguration = KeyValueConfigurationManager
                .Add(new KeyValueConfigurationAdapter(configurationRoot)).Build();

            SimpleCtorType actual = multiSourceKeyValueConfiguration.GetInstance<SimpleCtorType>();

            Assert.Equal(new SimpleCtorType("John", 42), actual, SimpleCtorType.NameAgeComparer);
        }

        [Fact]
        public void ItShouldUseValuesDefined()
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {["a:b:c"] = "123"}).Build();

            MultiSourceKeyValueConfiguration multiSourceKeyValueConfiguration = KeyValueConfigurationManager
                .Add(new KeyValueConfigurationAdapter(configurationRoot)).Build();

            string actual = multiSourceKeyValueConfiguration["a:b:c"];

            Assert.Equal("123", actual);
        }
    }
}