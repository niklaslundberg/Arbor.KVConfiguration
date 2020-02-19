using System.Collections.Generic;

namespace Arbor.KVConfiguration.Schema
{
    public class KeyValueConfigurationValidationSummary
    {
        public KeyValueConfigurationValidationSummary(
            [CanBeNull] IEnumerable<KeyValueConfigurationValidationResult> keyValueConfigurationValidationResults) =>
            KeyValueConfigurationValidationResults = keyValueConfigurationValidationResults.SafeToImmutableArray();

        public bool IsValid => KeyValueConfigurationValidationResults.All(result => result.IsValid);

        public ImmutableArray<KeyValueConfigurationValidationResult> KeyValueConfigurationValidationResults { get; }
    }
}