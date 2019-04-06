using System.Collections.Specialized;
using Arbor.KVConfiguration.Urns;
using Xunit;

namespace Arbor.KVConfiguration.Tests.Unit.Registrations
{
    public class RegistrationsTests3
    {
        [Fact]
        public void GettingRegistrationErrorsForMissingOptionalShouldReturnEmptyCollection()
        {
            var configuration = new Core.InMemoryKeyValueConfiguration(new NameValueCollection());

            ConfigurationRegistrations configurationRegistrations =
                configuration.GetRegistrations(typeof(ValidatableOptional));

            Assert.Empty(configurationRegistrations.UrnTypeRegistrations);
        }
    }
}
