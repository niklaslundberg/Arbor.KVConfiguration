using System;
using System.Collections.Generic;
using System.Linq;
using Arbor.KVConfiguration.Core;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Schema
{
    public class ConfigurationValidator : IConfigurationValidator
    {
        private readonly IReadOnlyCollection<IValueValidator> _validators;

        public ConfigurationValidator()
        {
            _validators = new List<IValueValidator>
            {
                new IntValidator(),
                new UriValidator(),
                new UrnValidator(),
                new BoolValidator(),
                new TimeSpanValidator()
            };
        }

        [UsedImplicitly]
        public ConfigurationValidator([NotNull] IReadOnlyCollection<IValueValidator> validators)
        {
            if (validators == null)
            {
                throw new ArgumentNullException(nameof(validators));
            }

            _validators = validators;
        }

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

            foreach (IValueValidator valueValidator in _validators)
            {
                if (!string.IsNullOrWhiteSpace(metadataItem.Metadata.ValueType) &&
                    multipleValuesStringPair.HasNonEmptyValue && multipleValuesStringPair.HasSingleValue)
                {
                    if (valueValidator.CanValidate(metadataItem.Metadata.ValueType))
                    {
                        string valueToValidate = multipleValuesStringPair.Values.SingleOrDefault();
                        validationErrors.AddRange(valueValidator.Validate(metadataItem.Metadata.ValueType,
                            valueToValidate));
                    }
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
