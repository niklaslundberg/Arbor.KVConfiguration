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

            var validationErrors = new List<ValidationError>();

            if (metadataItem.Metadata.IsRequired && string.IsNullOrWhiteSpace(metadataItem.Metadata.DefaultValue)
                && !multipleValuesStringPair.HasNonEmptyValue)
            {
                validationErrors.Add(new ValidationError("Required value is missing"));
            }

            if (metadataItem.Metadata.ValueType.Equals("uri", StringComparison.InvariantCultureIgnoreCase)
                && multipleValuesStringPair.HasNonEmptyValue && multipleValuesStringPair.HasSingleValue)
            {
                string uriString = multipleValuesStringPair.Values.Single();

                if (!Uri.IsWellFormedUriString(uriString, UriKind.RelativeOrAbsolute))
                {
                    validationErrors.Add(new ValidationError("Invalid URI"));
                }
            }

            if (metadataItem.Metadata.ValueType.Equals("urn", StringComparison.InvariantCultureIgnoreCase)
                && multipleValuesStringPair.HasNonEmptyValue && multipleValuesStringPair.HasSingleValue)
            {
                string uriString = multipleValuesStringPair.Values.Single();

                if (!Uri.IsWellFormedUriString(uriString, UriKind.RelativeOrAbsolute))
                {
                    validationErrors.Add(new ValidationError("Invalid URN"));
                }
                else
                {
                    Uri parsedUri;

                    bool parsed = Uri.TryCreate(uriString, UriKind.Absolute, out parsedUri);

                    if (!parsed || !parsedUri.IsAbsoluteUri
                        || !parsedUri.Scheme.Equals("urn", StringComparison.InvariantCultureIgnoreCase))
                    {
                        validationErrors.Add(new ValidationError("Invalid URN but valid URI"));
                    }
                }
            }

            if (metadataItem.Metadata.ValueType.Equals("bool", StringComparison.InvariantCultureIgnoreCase)
                && string.IsNullOrWhiteSpace(metadataItem.Metadata.DefaultValue))
            {
                bool parsedResult;

                if (!bool.TryParse(multipleValuesStringPair.Values.FirstOrDefault(), out parsedResult))
                {
                    validationErrors.Add(new ValidationError("Not a valid boolean value"));
                }
            }

            if (metadataItem.Metadata.ValueType.Equals("int", StringComparison.InvariantCultureIgnoreCase)
                && string.IsNullOrWhiteSpace(metadataItem.Metadata.DefaultValue))
            {
                int parsedResult;

                if (!int.TryParse(multipleValuesStringPair.Values.FirstOrDefault(), out parsedResult))
                {
                    validationErrors.Add(new ValidationError("Not a valid integer value"));
                }
            }

            if (metadataItem.Metadata.ValueType.Equals("timespan", StringComparison.InvariantCultureIgnoreCase)
                && string.IsNullOrWhiteSpace(metadataItem.Metadata.DefaultValue))
            {
                TimeSpan parsedResult;

                if (!TimeSpan.TryParse(multipleValuesStringPair.Values.FirstOrDefault(), out parsedResult))
                {
                    validationErrors.Add(new ValidationError("Not a valid timespan value"));
                }
            }

            var validationResult = new KeyValueConfigurationValidationResult(
                metadataItem,
                multipleValuesStringPair.Values,
                validationErrors);

            return validationResult;
        }
    }
}
