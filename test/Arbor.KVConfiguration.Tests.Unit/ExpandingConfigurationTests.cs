using System.Collections.Specialized;
using System.IO;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Core.Decorators;
using Arbor.KVConfiguration.Urns;
using JetBrains.Annotations;
using Xunit;

namespace Arbor.KVConfiguration.Tests.Unit
{
    public class ExpandingConfigurationTests
    {
        [Urn("urn:testinstance")]
        [UsedImplicitly]
        private class TestInstance
        {
            public TestInstance(string test)
            {
                Test = test;
            }

            public string Test { get; }
        }

        [Fact]
        public void ItShouldExpandEnvironmentVariables()
        {
            string tempPath = Path.GetTempPath();

            const string pattern = "%temp%\\";

            string valueWithPattern = $"{pattern} hello";

            var values = new NameValueCollection
            {
                ["Test"] = valueWithPattern
            };

            MultiSourceKeyValueConfiguration multiSourceKeyValueConfiguration = KeyValueConfigurationManager
                .Add(new Core.InMemoryKeyValueConfiguration(values))
                .DecorateWith(new ExpandKeyValueConfigurationDecorator())
                .Build();

            string expected = $"{tempPath} hello";
            Assert.Equal(expected, multiSourceKeyValueConfiguration["Test"]);
            Assert.Equal(expected, multiSourceKeyValueConfiguration.AllValues[0].Value);
            Assert.Equal(expected, multiSourceKeyValueConfiguration.AllWithMultipleValues[0].Values[0]);
            Assert.Equal(valueWithPattern, multiSourceKeyValueConfiguration.ConfigurationItems[0].Value);
        }

        [Fact]
        public void ItShouldExpandEnvironmentVariablesForInstances()
        {
            string tempPath = Path.GetTempPath();

            const string pattern = "%temp%\\";

            string valueWithPattern = $"{pattern} hello";

            string key = "urn:testinstance:0:test";

            var values = new NameValueCollection
            {
                [key] = valueWithPattern
            };

            MultiSourceKeyValueConfiguration multiSourceKeyValueConfiguration = KeyValueConfigurationManager
                .Add(new Core.InMemoryKeyValueConfiguration(values))
                .DecorateWith(new ExpandKeyValueConfigurationDecorator())
                .Build();

            string expected = $"{tempPath} hello";

            var testInstance = multiSourceKeyValueConfiguration.GetInstance<TestInstance>();

            Assert.NotNull(testInstance);

            Assert.Equal(expected, testInstance.Test);
        }
    }
}
