using System.Collections.Immutable;

namespace Arbor.KVConfiguration.Urns
{
    public class UrnTypeRegistration
    {
        public UrnTypeRegistration(
            UrnTypeMapping urnTypeMapping,
            INamedInstance<object> instance,
            params ConfigurationRegistrationError[] configurationRegistrationErrors)
        {
            Instance = instance;
            ConfigurationRegistrationErrors = configurationRegistrationErrors.ToImmutableArray();
            TypeMapping = urnTypeMapping;
        }

        public INamedInstance<object> Instance { get; }

        public ImmutableArray<ConfigurationRegistrationError> ConfigurationRegistrationErrors { get; }

        public UrnTypeMapping TypeMapping { get; }
    }
}
