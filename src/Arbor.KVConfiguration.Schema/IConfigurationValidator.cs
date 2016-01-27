using Arbor.KVConfiguration.Core;

namespace Arbor.KVConfiguration.Schema
{
    public interface IConfigurationValidator
    {
        KeyValueConfigurationValidationResult Validate(MultipleValuesStringPair multipleValuesStringPair, KeyMetadata metadataItem);
    }
}