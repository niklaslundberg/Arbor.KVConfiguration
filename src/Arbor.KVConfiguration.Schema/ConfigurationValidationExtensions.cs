using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Schema.Validators;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Schema
{
    public static class ConfigurationValidationExtensions
    {
        public static KeyValueConfigurationValidationSummary Validate(
            this ImmutableArray<MultipleValuesStringPair> multipleValuesStringPairs,
            [NotNull] IConfigurationValidator configurationValidator,
            ImmutableArray<KeyMetadata> metadata)
        {
            if (configurationValidator == null)
            {
                throw new ArgumentNullException(nameof(configurationValidator));
            }

            var keyValueConfigurationValidationResults = new List<KeyValueConfigurationValidationResult>();

            foreach (MultipleValuesStringPair multipleValuesStringPair in multipleValuesStringPairs)
            {
                KeyMetadata metadataItem =
                    metadata.SingleOrDefault(
                        item =>
                        item.Key.Equals(multipleValuesStringPair.Key, StringComparison.InvariantCultureIgnoreCase));

                KeyValueConfigurationValidationResult validationResult = configurationValidator.Validate(multipleValuesStringPair, metadataItem);

                if (!validationResult.IsValid)
                {
                    keyValueConfigurationValidationResults.Add(validationResult);
                }
            }

            return new KeyValueConfigurationValidationSummary(keyValueConfigurationValidationResults);
        }
    }
}
