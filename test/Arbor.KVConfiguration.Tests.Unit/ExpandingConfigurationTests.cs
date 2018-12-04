using System;
using System.Collections.Specialized;
using System.IO;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Core.Decorators;
using Xunit;

namespace Arbor.KVConfiguration.Tests.Unit
{
    public class ExpandingConfigurationTests
    {
        [Fact]
        public void ItShouldExpandEnvironmentVariables()
        {
            string tempPath = Path.GetTempPath();

            const string pattern = "%temp%\\";

            string valueWithPattern = $"{pattern} hello";

            var values = new NameValueCollection
            {
                ["Test"]= valueWithPattern
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
    }
}
