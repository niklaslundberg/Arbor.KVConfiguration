using System.Collections.Immutable;

namespace Arbor.KVConfiguration.Urns
{
    public class ConfigurationRegistrations
    {
        public ConfigurationRegistrations(ImmutableArray<UrnTypeRegistration> urnTypeRegistrations) =>
            UrnTypeRegistrations = urnTypeRegistrations;

        public ImmutableArray<UrnTypeRegistration> UrnTypeRegistrations { get; }
    }
}