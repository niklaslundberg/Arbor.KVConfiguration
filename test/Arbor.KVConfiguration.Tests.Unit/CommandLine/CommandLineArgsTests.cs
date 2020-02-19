using System.Linq;
using Arbor.KVConfiguration.Core.Extensions.CommandLine;
using Xunit;

namespace Arbor.KVConfiguration.Tests.Unit.CommandLine
{
    public class CommandLineArgsTests
    {
        [Fact]
        public void WhenParsingMultipleSimpleValueWithSameKeyItShouldBeResolvableInConfiguration()
        {
            string[] args = {"abc=123", "abc=123"};

            var keyValueConfiguration = args.ToKeyValueConfiguration();

            string[] actualStrings = keyValueConfiguration.AllWithMultipleValues[0].Values.ToArray();
            Assert.Equal(new[] {"123", "123"}, actualStrings);
        }

        [Fact]
        public void WhenParsingSimpleValueItShouldBeResolvableInConfiguration()
        {
            string[] args = {"abc=123"};

            var keyValueConfiguration = args.ToKeyValueConfiguration();

            Assert.Equal("123", keyValueConfiguration["abc"]);
        }
    }
}