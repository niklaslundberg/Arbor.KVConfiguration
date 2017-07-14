using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Arbor.KVConfiguration.Core.Metadata;
using Arbor.KVConfiguration.Schema.Validators;

namespace Arbor.KVConfiguration.Schema
{
    public class KeyValueConfigurationValidationResult
    {
        public KeyValueConfigurationValidationResult(
            KeyMetadata keyMetadata,
            IEnumerable<string> values,
            IEnumerable<ValidationError> validationErrors = null)
        {
            KeyMetadata = keyMetadata;
            Values = values.SafeToImmutableArray();
            ValidationErrors = validationErrors?.ToImmutableArray() ?? ImmutableArray<ValidationError>.Empty;
        }

        public KeyMetadata KeyMetadata { get; }

        public ImmutableArray<string> Values { get; }

        public ImmutableArray<ValidationError> ValidationErrors { get; }

        public bool IsValid => !ValidationErrors.Any();
    }
}
