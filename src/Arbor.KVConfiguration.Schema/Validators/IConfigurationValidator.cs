using Arbor.KVConfiguration.Core;

namespace Arbor.KVConfiguration.Schema.Validators
{
    public interface IConfigurationValidator
    {
        KeyValueConfigurationValidationResult Validate(
            MultipleValuesStringPair multipleValuesStringPair,
            KeyMetadata metadataItem);
    }
}
