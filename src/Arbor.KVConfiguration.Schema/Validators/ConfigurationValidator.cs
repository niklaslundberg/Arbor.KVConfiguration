using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Core.Metadata;

namespace Arbor.KVConfiguration.Schema.Validators;

public class ConfigurationValidator : IConfigurationValidator
{
    private readonly ImmutableArray<IValueValidator> _validators;

    public ConfigurationValidator() =>
        _validators = [
            new IntValidator(), new UriValidator(), new BoolValidator(), new TimeSpanValidator()
        ];

    
    public ConfigurationValidator(ImmutableArray<IValueValidator> validators) =>
        _validators = validators.ThrowIfDefault();

    public KeyValueConfigurationValidationResult Validate(
        MultipleValuesStringPair multipleValuesStringPair,
        KeyMetadata metadataItem)
    {
        metadataItem.ThrowIfNull(nameof(metadataItem));

        if (metadataItem.ConfigurationMetadata is null)
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
                multipleValuesStringPair.HasNonEmptyValue && multipleValuesStringPair.HasSingleValue && valueValidator.CanValidate(metadataItem.ConfigurationMetadata.ValueType))
            {
                string? valueToValidate = multipleValuesStringPair.Values.SingleOrDefault();

                validationErrors.AddRange(valueValidator.Validate(metadataItem.ConfigurationMetadata.ValueType,
                    valueToValidate));
            }
        }

        var validationResult = new KeyValueConfigurationValidationResult(
            metadataItem,
            multipleValuesStringPair.Values,
            validationErrors);

        return validationResult;
    }
}