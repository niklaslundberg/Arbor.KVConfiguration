using System;
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
        private readonly ITestOutputHelper output;
        public RegistrationsTests(ITestOutputHelper output) => this.output = output;

        [Fact]
        public void WhenGettingRegistrationErrorsForMissingOptionalShouldReturnEmptyCollection()
        {
            var configuration = NoConfiguration.Empty;

            var configurationRegistrations =
                configuration.GetRegistrations(typeof(ValidatableOptional));

            Assert.Empty(configurationRegistrations.UrnTypeRegistrations);
        }

        [Fact]
        public void WhenGettingRegistrationErrorsForMissingRequiredShouldReturnNonEmptyCollection()
        {
            var configuration = NoConfiguration.Empty;

            void Handle(Exception ex)
            {
                output.WriteLine(ex.ToString());
            }

            var configurationRegistrations = configuration.ScanRegistrations(Handle, AppDomain.CurrentDomain.GetAssemblies());

            foreach (UrnTypeRegistration urnTypeRegistration in configurationRegistrations.UrnTypeRegistrations.Where(
                s => s.ConfigurationRegistrationErrors.Length > 0))
            {
                foreach (var configurationRegistrationError in urnTypeRegistration
                    .ConfigurationRegistrationErrors)
                {
                    output.WriteLine(configurationRegistrationError.ErrorMessage);
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
                [ValidatableRequired.Urn + ":myInstance:value"] = "-42"
            });

            var configurationRegistrations = configuration.GetRegistrations(typeof(ValidatableRequired));

            foreach (UrnTypeRegistration configurationRegistrationsUrnTypeRegistration in configurationRegistrations
                .UrnTypeRegistrations.Where(s => s.ConfigurationRegistrationErrors.Length > 0))
            {
                foreach (var configurationRegistrationError in configurationRegistrationsUrnTypeRegistration
                    .ConfigurationRegistrationErrors)
                {
                    output.WriteLine("Invalid instance {0}, error message: '{1}'",
                        configurationRegistrationsUrnTypeRegistration.Instance,
                        configurationRegistrationError.ErrorMessage);
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
                [ValidatableRequired.Urn + ":myInstance2:value"] = "7"
            });

            var configurationRegistrations = configuration.GetRegistrations(typeof(ValidatableRequired));

            foreach (UrnTypeRegistration configurationRegistrationsUrnTypeRegistration in configurationRegistrations
                .UrnTypeRegistrations.Where(s => s.ConfigurationRegistrationErrors.Length > 0))
            {
                foreach (var configurationRegistrationError in configurationRegistrationsUrnTypeRegistration
                    .ConfigurationRegistrationErrors)
                {
                    output.WriteLine("Invalid, error message: '{0}'", configurationRegistrationError.ErrorMessage);
                }
            }

            Assert.Equal(2, configurationRegistrations.UrnTypeRegistrations.Length);

            Assert.Equal(1,
                configurationRegistrations.UrnTypeRegistrations.Count(registration =>
                    registration.ConfigurationRegistrationErrors.Any()));

            Assert.Equal(1,
                configurationRegistrations.UrnTypeRegistrations.Count(registration =>
                    !registration.ConfigurationRegistrationErrors.Any()));
        }
    }
}