using System;
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
        [Fact]
        public void ItShouldExpandCustomEnvironmentVariables()
        {
            Environment.SetEnvironmentVariable("abcvalue", "123");


            var values = new NameValueCollection {["Test"] = "%abcvalue%"};

            var multiSourceKeyValueConfiguration = KeyValueConfigurationManager
                .Add(new Core.InMemoryKeyValueConfiguration(values))
                .DecorateWith(new ExpandKeyValueConfigurationDecorator())
                .Build();

            string expanded = multiSourceKeyValueConfiguration["test"];

            Assert.Equal("123", expanded);
        }

        [Fact]
        public void ItShouldExpandEnvironmentVariables()
        {
            string tempPath = Path.GetTempPath();

            const string pattern = "%temp%\\";

            string valueWithPattern = $"{pattern} hello";

            var values = new NameValueCollection {["Test"] = valueWithPattern};

            var multiSourceKeyValueConfiguration = KeyValueConfigurationManager
                .Add(new Core.InMemoryKeyValueConfiguration(values))
                .DecorateWith(new ExpandKeyValueConfigurationDecorator())
                .Build();

            static string FullPath(string path)
            {
                return Path.GetFullPath(path);
            }

            string expected = $"{tempPath} hello";
            Assert.Equal(expected, FullPath(multiSourceKeyValueConfiguration["Test"]));
            Assert.Equal(expected, FullPath(multiSourceKeyValueConfiguration.AllValues[0].Value));
            Assert.Equal(expected, FullPath(multiSourceKeyValueConfiguration.AllWithMultipleValues[0].Values[0]));
            Assert.Equal(valueWithPattern, multiSourceKeyValueConfiguration.ConfigurationItems[0].Value);
        }

        [Fact]
        public void ItShouldExpandEnvironmentVariablesForInstances()
        {
            static string FullPath(string path)
            {
                return Path.GetFullPath(path);
            }

            string tempPath = Path.GetTempPath();

            const string pattern = "%temp%\\";

            string valueWithPattern = $"{pattern} hello";

            string key = "urn:test:testinstance:0:test";

            var values = new NameValueCollection {[key] = valueWithPattern};

            var multiSourceKeyValueConfiguration = KeyValueConfigurationManager
                .Add(new Core.InMemoryKeyValueConfiguration(values))
                .DecorateWith(new ExpandKeyValueConfigurationDecorator())
                .Build();

            string expected = $"{tempPath} hello";

            var testInstance = multiSourceKeyValueConfiguration.GetInstance<TestInstance>();

            Assert.NotNull(testInstance);

            Assert.Equal(expected, FullPath(testInstance.Test));
        }

        [Urn("urn:test:testinstance")]
        [UsedImplicitly]
        private class TestInstance
        {
            public TestInstance(string test) => Test = test;

            public string Test { get; }
        }
    }
}