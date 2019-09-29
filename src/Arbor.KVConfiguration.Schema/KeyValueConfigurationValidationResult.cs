using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Core.Metadata;

namespace Arbor.KVConfiguration.Schema
{
    public class KeyValueConfigurationValidationResult
    {
        public KeyValueConfigurationValidationResult(
            KeyMetadata keyMetadata,
            IEnumerable<string> values,
            IEnumerable<ValidationError>? validationErrors = default)
        {
            KeyMetadata = keyMetadata;
            Values = values.SafeToImmutableArray();
            ValidationErrors = validationErrors!.SafeToImmutableArray();
        }

        public KeyMetadata KeyMetadata { get; }

        public ImmutableArray<string> Values { get; }

        public ImmutableArray<ValidationError> ValidationErrors { get; }

        public bool IsValid => !ValidationErrors.Any();
    }
}
