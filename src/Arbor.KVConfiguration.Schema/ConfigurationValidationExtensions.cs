using System;
using System.Collections.Generic;
using System.Linq;

using Arbor.KVConfiguration.Core;

using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Schema
{
    public static class ConfigurationValidationExtensions
    {
        public static KeyValueConfigurationValidationSummary Validate(
            [NotNull] this IReadOnlyCollection<MultipleValuesStringPair> multipleValuesStringPairs,
            [NotNull] IConfigurationValidator configurationValidator,
            [NotNull] IReadOnlyCollection<KeyMetadata> metadata)
        {
            if (configurationValidator == null)
            {
                throw new ArgumentNullException(nameof(configurationValidator));
            }
            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            if (multipleValuesStringPairs == null)
            {
                throw new ArgumentNullException(nameof(multipleValuesStringPairs));
            }

            var keyValueConfigurationValidationResults = new List<KeyValueConfigurationValidationResult>();

            foreach (MultipleValuesStringPair multipleValuesStringPair in multipleValuesStringPairs)
            {
                var metadataItem =
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
