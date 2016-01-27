using System;
using System.Collections.Generic;
using System.Linq;

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
            Values = values?.ToArray() ?? new string[] { };
            ValidationErrors = validationErrors?.ToArray() ?? new ValidationError[] { };
        }

        public KeyMetadata KeyMetadata { get; }

        public IReadOnlyCollection<string> Values { get; }

        public IReadOnlyCollection<ValidationError> ValidationErrors { get; }

        public bool IsValid => !ValidationErrors.Any();
    }
}
