using System.Collections.Generic;
using System.Linq;

namespace Arbor.KVConfiguration.Schema
{
    public class KeyValueConfigurationValidationSummary
    {
        public KeyValueConfigurationValidationSummary(
            IEnumerable<KeyValueConfigurationValidationResult> keyValueConfigurationValidationResults)
        {
            KeyValueConfigurationValidationResults = keyValueConfigurationValidationResults?.ToArray()
                                                     ?? new KeyValueConfigurationValidationResult[]
                                                            {
                                                            };
        }

        public bool IsValid => KeyValueConfigurationValidationResults.All(_ => _.IsValid);

        public IReadOnlyCollection<KeyValueConfigurationValidationResult> KeyValueConfigurationValidationResults { get;
        }
    }
}
