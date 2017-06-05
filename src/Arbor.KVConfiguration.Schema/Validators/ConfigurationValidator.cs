﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Arbor.KVConfiguration.Core;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Schema.Validators
{
    public class ConfigurationValidator : IConfigurationValidator
    {
        private readonly ImmutableArray<IValueValidator> _validators;

        public ConfigurationValidator()
        {
            _validators = new List<IValueValidator>(10)
            {
                new IntValidator(),
                new UriValidator(),
                new UrnValidator(),
                new BoolValidator(),
                new TimeSpanValidator()
            }.ToImmutableArray();
        }

        [UsedImplicitly]
        public ConfigurationValidator([NotNull] IEnumerable<IValueValidator> validators)
        {
            _validators = validators?.ToImmutableArray() ?? throw new ArgumentNullException(nameof(validators));
        }

        public KeyValueConfigurationValidationResult Validate(
            MultipleValuesStringPair multipleValuesStringPair,
            KeyMetadata metadataItem)
        {
            if (metadataItem.ConfigurationMetadata == null)
            {
                return new KeyValueConfigurationValidationResult(metadataItem, multipleValuesStringPair.Values);
            }

            var validationErrors = new List<ValidationError>();

            if (metadataItem.ConfigurationMetadata.IsRequired &&
                string.IsNullOrWhiteSpace(metadataItem.ConfigurationMetadata.DefaultValue)
                && !multipleValuesStringPair.HasNonEmptyValue)
            {
                validationErrors.Add(new ValidationError("Required value is missing"));
            }

            foreach (IValueValidator valueValidator in _validators)
            {
                if (!string.IsNullOrWhiteSpace(metadataItem.ConfigurationMetadata.ValueType) &&
                    multipleValuesStringPair.HasNonEmptyValue && multipleValuesStringPair.HasSingleValue)
                {
                    if (valueValidator.CanValidate(metadataItem.ConfigurationMetadata.ValueType))
                    {
                        string valueToValidate = multipleValuesStringPair.Values.SingleOrDefault();
                        validationErrors.AddRange(valueValidator.Validate(metadataItem.ConfigurationMetadata.ValueType,
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