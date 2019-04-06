using System.Collections.Specialized;
using System.Linq;
using Arbor.KVConfiguration.Urns;
using Xunit;
using Xunit.Abstractions;

namespace Arbor.KVConfiguration.Tests.Unit.Registrations
{
    public class RegistrationsTests2
    {
        private readonly ITestOutputHelper output;

        public RegistrationsTests2(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Do()
        {
            Core.InMemoryKeyValueConfiguration configuration = new Core.InMemoryKeyValueConfiguration(new NameValueCollection
            {
                [ValidatableRequired.Urn + ":myInstance:name"] = null,
                [ValidatableRequired.Urn + ":myInstance:value"] = "-42",
            });

            ConfigurationRegistrations configurationRegistrations = configuration.GetRegistrations(typeof(ValidatableRequired));

            foreach (UrnTypeRegistration configurationRegistrationsUrnTypeRegistration in configurationRegistrations.UrnTypeRegistrations.Where(s => s.ConfigurationRegistrationErrors.Length > 0))
            {
                foreach (ConfigurationRegistrationError configurationRegistrationError in configurationRegistrationsUrnTypeRegistration.ConfigurationRegistrationErrors)
                {
                    output.WriteLine("Invalid instance {0}, error message: '{1}'", configurationRegistrationsUrnTypeRegistration.Instance, configurationRegistrationError.Error);
                }
            }

            Assert.NotEmpty(configurationRegistrations.UrnTypeRegistrations);
        }
        [Fact]
        public void Mixed()
        {
            Core.InMemoryKeyValueConfiguration configuration = new Core.InMemoryKeyValueConfiguration(new NameValueCollection
            {
                [ValidatableRequired.Urn + ":myInstance1:name"] = null,
                [ValidatableRequired.Urn + ":myInstance1:value"] = "-42",
                [ValidatableRequired.Urn + ":myInstance2:name"] = "abc",
                [ValidatableRequired.Urn + ":myInstance2:value"] = "7",
            });

            ConfigurationRegistrations configurationRegistrations = configuration.GetRegistrations(typeof(ValidatableRequired));

            foreach (UrnTypeRegistration configurationRegistrationsUrnTypeRegistration in configurationRegistrations.UrnTypeRegistrations.Where(s => s.ConfigurationRegistrationErrors.Length > 0))
            {
                foreach (ConfigurationRegistrationError configurationRegistrationError in configurationRegistrationsUrnTypeRegistration.ConfigurationRegistrationErrors)
                {
                    output.WriteLine("Invalid, error message: '{0}'", configurationRegistrationError.Error);
                }
            }

            Assert.NotEmpty(configurationRegistrations.UrnTypeRegistrations);
        }
    }
}