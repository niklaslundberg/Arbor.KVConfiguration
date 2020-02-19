using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;

namespace Arbor.KVConfiguration.Urns
{
    public class UrnTypeRegistration
    {
        public UrnTypeRegistration(
            UrnTypeMapping urnTypeMapping,
            INamedInstance<object>? instance,
            params ValidationResult[] configurationRegistrationErrors)
        {
            Instance = instance;
            ConfigurationRegistrationErrors = configurationRegistrationErrors.ToImmutableArray();
            TypeMapping = urnTypeMapping;
        }

        public INamedInstance<object>? Instance { get; }

        public ImmutableArray<ValidationResult> ConfigurationRegistrationErrors { get; }

        public UrnTypeMapping TypeMapping { get; }
    }
}