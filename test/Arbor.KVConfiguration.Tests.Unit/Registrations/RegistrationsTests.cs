using System.Collections.Specialized;
using System.Linq;
using Arbor.KVConfiguration.Tests.Unit.Urn;
using Arbor.KVConfiguration.Urns;
using Xunit;
using Xunit.Abstractions;

namespace Arbor.KVConfiguration.Tests.Unit.Registrations
{
    public class RegistrationsTests
    {
        private readonly ITestOutputHelper output;

        public RegistrationsTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Do()
        {
            Core.InMemoryKeyValueConfiguration configuration = new Core.InMemoryKeyValueConfiguration(new NameValueCollection
            {
                //[ValidatableRequired.Urn + "myInstance:name"] = null
            });

            ConfigurationRegistrations configurationRegistrations = configuration.ScanRegistrations();

            foreach (UrnTypeRegistration configurationRegistrationsUrnTypeRegistration in configurationRegistrations.UrnTypeRegistrations.Where(s => s.ConfigurationRegistrationErrors.Length > 0))
            {
                foreach (ConfigurationRegistrationError configurationRegistrationError in configurationRegistrationsUrnTypeRegistration.ConfigurationRegistrationErrors)
                {
                    output.WriteLine(configurationRegistrationError.Error);
                }
            }

            Assert.NotEmpty(configurationRegistrations.UrnTypeRegistrations);
        }
    }
}
