using System;
using System.Collections.Generic;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Core.Metadata;
using Arbor.KVConfiguration.Schema.Validators;

namespace Arbor.KVConfiguration.Schema
{
    public static class ConfigurationValidationExtensions
    {
        public static KeyValueConfigurationValidationSummary Validate(
            this ImmutableArray<MultipleValuesStringPair> multipleValuesStringPairs,
            [NotNull] IConfigurationValidator configurationValidator,
            ImmutableArray<KeyMetadata> metadata)
        {
            if (configurationValidator is null)
            {
                throw new ArgumentNullException(nameof(configurationValidator));
            }

            var keyValueConfigurationValidationResults = new List<KeyValueConfigurationValidationResult>();

            foreach (MultipleValuesStringPair multipleValuesStringPair in multipleValuesStringPairs)
            {
                KeyMetadata metadataItem =
                    metadata.SafeToImmutableArray().SingleOrDefault(
                        item =>
                            item.Key.Equals(multipleValuesStringPair.Key, StringComparison.OrdinalIgnoreCase));

                if (metadataItem is object)
                {
                    var validationResult =
                        configurationValidator.Validate(multipleValuesStringPair, metadataItem);

                    if (!validationResult.IsValid)
                    {
                        keyValueConfigurationValidationResults.Add(validationResult);
                    }
                }
            }

            return new KeyValueConfigurationValidationSummary(keyValueConfigurationValidationResults);
        }
    }
}