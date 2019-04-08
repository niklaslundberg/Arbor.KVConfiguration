using System.Collections.Immutable;

namespace Arbor.KVConfiguration.Urns
{
    public class ConfigurationRegistrations
    {
        public ImmutableArray<UrnTypeRegistration> UrnTypeRegistrations { get; }

        public ConfigurationRegistrations(ImmutableArray<UrnTypeRegistration> urnTypeRegistrations)
        {
            UrnTypeRegistrations = urnTypeRegistrations;
        }
    }
}
