using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Specialized;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Microsoft.Extensions.Configuration.Urns;
using Arbor.KVConfiguration.Urns;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Arbor.KVConfiguration.Tests.Integration
{
    public class WhenUsingIKeyValueConfigurationAsSourceInMicrosoftConfiguration
    {
        [Fact]
        public void ItShouldResolveMultipleInstancesFromUrn()
        {
            var inMemoryKeyValueConfiguration =
                new InMemoryKeyValueConfiguration(new NameValueCollection
                {
                    {"urn:test:simple:instance:name", "John"}, {"urn:test:simple:instance:age", "42"}
                });

            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["urn:test:simple:agent:name"] = "James", ["urn:test:simple:agent:age"] = "50"
                })
                .AddKeyValueConfigurationSource(inMemoryKeyValueConfiguration).Build();

            ImmutableArray<SimpleCtorType> simpleCtorType =
                configurationRoot.ToKeyValueConfigurator().GetInstances<SimpleCtorType>();

            Assert.NotEmpty(simpleCtorType);

            Assert.Contains(new SimpleCtorType("John", 42), simpleCtorType, SimpleCtorType.NameAgeComparer);
            Assert.Contains(new SimpleCtorType("James", 50), simpleCtorType, SimpleCtorType.NameAgeComparer);
        }

        [Fact]
        public void ItShouldResolveSingleInstanceFromUrn()
        {
            var inMemoryKeyValueConfiguration =
                new InMemoryKeyValueConfiguration(new NameValueCollection
                {
                    {"urn:test:simple:instance:name", "John"}, {"urn:test:simple:instance:age", "42"}
                });

            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                .AddKeyValueConfigurationSource(inMemoryKeyValueConfiguration).Build();

            SimpleCtorType simpleCtorType = configurationRoot.ToKeyValueConfigurator().GetInstance<SimpleCtorType>();

            Assert.NotNull(simpleCtorType);
            Assert.Equal("John", simpleCtorType.Name);
            Assert.Equal(42, simpleCtorType.Age);
        }

        [Fact]
        public void ItShouldUseOverrideValues()
        {
            var inMemoryKeyValueConfiguration =
                new InMemoryKeyValueConfiguration(new NameValueCollection {{"a:b:c", "234"}});

            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {["a:b:c"] = "123"})
                .AddKeyValueConfigurationSource(inMemoryKeyValueConfiguration).Build();

            IConfigurationSection configurationSection = configurationRoot.GetSection("a");

            IConfigurationSection subSection = configurationSection.GetSection("b");

            IConfigurationSection subSubSection = subSection.GetSection("c");

            Assert.Equal("c", subSubSection.Key);
            Assert.Equal("a:b:c", subSubSection.Path);
            Assert.Equal("234", subSubSection.Value);
        }

        [Fact]
        public void ItShouldUseValuesDefined()
        {
            var inMemoryKeyValueConfiguration =
                new InMemoryKeyValueConfiguration(new NameValueCollection {{"a:b:d", "234"}});

            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {["a:b:c"] = "123"})
                .AddKeyValueConfigurationSource(inMemoryKeyValueConfiguration).Build();

            IConfigurationSection configurationSection = configurationRoot.GetSection("a");

            Assert.Equal("a", configurationSection.Key);
            Assert.Equal("a", configurationSection.Path);

            IConfigurationSection subSection = configurationSection.GetSection("b");

            Assert.Equal("b", subSection.Key);
            Assert.Equal("a:b", subSection.Path);

            IConfigurationSection subSubSection = subSection.GetSection("c");

            Assert.Equal("c", subSubSection.Key);
            Assert.Equal("a:b:c", subSubSection.Path);
            Assert.Equal("123", subSubSection.Value);

            IConfigurationSection subSubSectionD = subSection.GetSection("d");

            Assert.Equal("d", subSubSectionD.Key);
            Assert.Equal("a:b:d", subSubSectionD.Path);
            Assert.Equal("234", subSubSectionD.Value);
        }
    }
}