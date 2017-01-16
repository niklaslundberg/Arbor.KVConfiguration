using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Schema
{
    public class KeyValueConfigurationValidationSummary
    {
        public KeyValueConfigurationValidationSummary(
            [CanBeNull] IEnumerable<KeyValueConfigurationValidationResult> keyValueConfigurationValidationResults)
        {
            KeyValueConfigurationValidationResults = keyValueConfigurationValidationResults?.ToImmutableArray() ??
                                                     ImmutableArray<KeyValueConfigurationValidationResult>.Empty;
        }

        public bool IsValid => KeyValueConfigurationValidationResults.All(_ => _.IsValid);

        public ImmutableArray<KeyValueConfigurationValidationResult> KeyValueConfigurationValidationResults { get; }
    }
}
