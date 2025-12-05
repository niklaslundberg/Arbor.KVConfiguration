using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Arbor.KVConfiguration.Schema;

public class KeyValueConfigurationValidationSummary
{
    public KeyValueConfigurationValidationSummary(
        IEnumerable<KeyValueConfigurationValidationResult>? keyValueConfigurationValidationResults) =>
        KeyValueConfigurationValidationResults = keyValueConfigurationValidationResults.SafeToImmutableArray();

    public bool IsValid => KeyValueConfigurationValidationResults.All(result => result.IsValid);

    public ImmutableArray<KeyValueConfigurationValidationResult> KeyValueConfigurationValidationResults { get; }
}