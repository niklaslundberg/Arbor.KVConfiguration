using System.Collections.Immutable;
using System.Linq;
using Arbor.KVConfiguration.Urns;
using Xunit;

namespace Arbor.KVConfiguration.Tests.Unit.Registrations
{
    public class ConfigurationHolderTests
    {
        [Fact]
        public void WhenRegisteringMultipleInstances()
        {
            var holder = new ConfigurationInstanceHolder();
            holder.Add(new NamedInstance<ValidatableOptional>(new ValidatableOptional("abc", 123), "abc-instance"));
            holder.Add(new NamedInstance<ValidatableOptional>(new ValidatableOptional("def", 234), "def-instance"));

            ImmutableDictionary<string, ValidatableOptional> instances = holder.GetInstances<ValidatableOptional>();

            Assert.Equal(2, instances.Count);

            Assert.Contains("abc-instance", instances.Keys);
            Assert.Equal("abc", instances["abc-instance"].Name);
            Assert.Equal(123, instances["abc-instance"].Value);

            Assert.Contains("def-instance", instances.Keys);
            Assert.Equal("def", instances["def-instance"].Name);
            Assert.Equal(234, instances["def-instance"].Value);
        }

        [Fact]
        public void WhenRegisteringSingleInstance()
        {
            var holder = new ConfigurationInstanceHolder();
            holder.Add(new NamedInstance<ValidatableOptional>(new ValidatableOptional("abc", 123), "abc-instance"));

            ImmutableDictionary<string, ValidatableOptional> instances = holder.GetInstances<ValidatableOptional>();

            Assert.Single(instances);

            Assert.Equal("abc-instance", instances.Keys.Single());
            Assert.Equal("abc", instances["abc-instance"].Name);
            Assert.Equal(123, instances["abc-instance"].Value);
        }

        [Fact]
        public void WhenRegisteringSingleInstanceTryGet()
        {
            var holder = new ConfigurationInstanceHolder();
            holder.Add(new NamedInstance<ValidatableOptional>(new ValidatableOptional("abc", 123), "abc-instance"));

            bool found = holder.TryGet("abc-instance", out ValidatableOptional _);

            Assert.True(found);
        }
    }
}
