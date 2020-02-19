using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Core.Metadata;

namespace Arbor.KVConfiguration.Schema.Validators
{
    public interface IConfigurationValidator
    {
        KeyValueConfigurationValidationResult Validate(
            MultipleValuesStringPair multipleValuesStringPair,
            KeyMetadata metadataItem);
    }
}