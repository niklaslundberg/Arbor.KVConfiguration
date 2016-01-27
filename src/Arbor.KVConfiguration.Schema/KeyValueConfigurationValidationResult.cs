using System.Collections.Generic;
using System.Linq;

namespace Arbor.KVConfiguration.Schema
{
    public class KeyValueConfigurationValidationResult
    {
        public KeyValueConfigurationValidationResult(
            KeyMetadata keyMetadata,
            string value,
            IEnumerable<ValidationError> validationErrors = null)
        {
            KeyMetadata = keyMetadata;
            Value = value;
            ValidationErrors = validationErrors?.ToArray() ?? new ValidationError[] { };
        }

        public KeyMetadata KeyMetadata { get; }

        public string Value { get; }

        public IReadOnlyCollection<ValidationError> ValidationErrors { get; }

        public bool IsValid => !ValidationErrors.Any();
    }
}
