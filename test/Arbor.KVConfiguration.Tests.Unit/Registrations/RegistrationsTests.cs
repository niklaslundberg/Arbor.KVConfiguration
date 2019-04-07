using System.Collections.Specialized;
using System.Linq;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Urns;
using Xunit;
using Xunit.Abstractions;

namespace Arbor.KVConfiguration.Tests.Unit.Registrations
{
    public class RegistrationsTests
    {
        public RegistrationsTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        private readonly ITestOutputHelper output;

        [Fact]
        public void WhenGettingRegistrationErrorsForMissingOptionalShouldReturnEmptyCollection()
        {
            IKeyValueConfiguration configuration = NoConfiguration.Empty;

            ConfigurationRegistrations configurationRegistrations =
                configuration.GetRegistrations(typeof(ValidatableOptional));

            Assert.Empty(configurationRegistrations.UrnTypeRegistrations);
        }

        [Fact]
        public void WhenGettingRegistrationErrorsForMissingRequiredShouldReturnNonEmptyCollection()
        {
            IKeyValueConfiguration configuration = NoConfiguration.Empty;

            ConfigurationRegistrations configurationRegistrations = configuration.ScanRegistrations();

            foreach (UrnTypeRegistration urnTypeRegistration in configurationRegistrations.UrnTypeRegistrations.Where(
                s => s.ConfigurationRegistrationErrors.Length > 0))
            {
                foreach (ConfigurationRegistrationError configurationRegistrationError in urnTypeRegistration
                    .ConfigurationRegistrationErrors)
                {
                    output.WriteLine(configurationRegistrationError.Error);
                }
            }

            Assert.NotEmpty(configurationRegistrations.UrnTypeRegistrations);
        }

        [Fact]
        public void WhenGettingRegistrationForInvalidInstanceItShouldReturnNonEmptyErrorCollection()
        {
            IKeyValueConfiguration configuration = new Core.InMemoryKeyValueConfiguration(new NameValueCollection
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
        public void WhenGettingRegistrationsForValidAndInvalidInstancesItShouldReturnAllInstancesAndHaveErrors()
        {
            IKeyValueConfiguration configuration = new Core.InMemoryKeyValueConfiguration(new NameValueCollection
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

            Assert.Equal(2, configurationRegistrations.UrnTypeRegistrations.Length);
            Assert.Equal(1, configurationRegistrations.UrnTypeRegistrations.Count(registration => registration.ConfigurationRegistrationErrors.Any()));
            Assert.Equal(1, configurationRegistrations.UrnTypeRegistrations.Count(registration => !registration.ConfigurationRegistrationErrors.Any()));
        }
    }
}
