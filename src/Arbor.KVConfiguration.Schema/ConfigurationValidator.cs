using System;
using System.Collections.Generic;
using System.Linq;

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

            if (metadataItem.Metadata.ValueType.Equals("uri", StringComparison.InvariantCultureIgnoreCase)
                && multipleValuesStringPair.HasNonEmptyValue && multipleValuesStringPair.HasSingleValue)
            {
                Uri parsedUri;
                if (!Uri.TryCreate(multipleValuesStringPair.Values.Single(), UriKind.RelativeOrAbsolute, out parsedUri))
                {
                    validationErrors.Add(new ValidationError("Invalid URI"));
                }
            }

            if (metadataItem.Metadata.ValueType.Equals("urn", StringComparison.InvariantCultureIgnoreCase)
                && multipleValuesStringPair.HasNonEmptyValue && multipleValuesStringPair.HasSingleValue)
            {
                Uri parsedUri;
                if (!Uri.TryCreate(multipleValuesStringPair.Values.Single(), UriKind.RelativeOrAbsolute, out parsedUri))
                {
                    validationErrors.Add(new ValidationError("Invalid URN"));
                }

                if (!parsedUri.IsAbsoluteUri
                    || !parsedUri.Scheme.Equals("urn", StringComparison.InvariantCultureIgnoreCase))
                {
                    validationErrors.Add(new ValidationError("Invalid URN but valid URI"));
                }
            }

            return new KeyValueConfigurationValidationResult(
                metadataItem,
                multipleValuesStringPair.Values,
                validationErrors);
        }
    }
}
