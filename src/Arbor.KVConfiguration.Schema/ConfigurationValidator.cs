using System.Collections.Generic;

using Arbor.KVConfiguration.Core;

namespace Arbor.KVConfiguration.Schema
{
    public class ConfigurationValidator : IConfigurationValidator
    {
        public KeyValueConfigurationValidationResult Validate(
            MultipleValuesStringPair multipleValuesStringPair,
            KeyMetadata metadataItem)
        {
            if (metadataItem.Metadata == null)
            {
                return new KeyValueConfigurationValidationResult(metadataItem, multipleValuesStringPair.Values);
            }

            List<ValidationError> validationErrors = new List<ValidationError>();

            if (metadataItem.Metadata.IsRequired && string.IsNullOrWhiteSpace(metadataItem.Metadata.DefaultValue)
                && !multipleValuesStringPair.HasNonEmptyValue)
            {
                validationErrors.Add(new ValidationError("Required value is missing"));
            }

            return new KeyValueConfigurationValidationResult(
                metadataItem,
                multipleValuesStringPair.Values,
                validationErrors);
        }
    }
}
