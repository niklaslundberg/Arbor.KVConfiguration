using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Schema
{
    public class KeyValueConfigurationValidationSummary
    {
        public KeyValueConfigurationValidationSummary(
            [CanBeNull] IEnumerable<KeyValueConfigurationValidationResult> keyValueConfigurationValidationResults)
        {
            KeyValueConfigurationValidationResults = keyValueConfigurationValidationResults?.ToArray()
                                                     ?? ArrayExtensions<KeyValueConfigurationValidationResult>.Empty();
        }

        public bool IsValid => KeyValueConfigurationValidationResults.All(_ => _.IsValid);

        public IReadOnlyCollection<KeyValueConfigurationValidationResult> KeyValueConfigurationValidationResults { get;
        }
    }
}
