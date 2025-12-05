using System;

namespace Arbor.KVConfiguration.Core.Metadata;

public class KeyMetadata
{
    public KeyMetadata(string key, ConfigurationMetadata? configurationMetadata)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException(KeyValueResources.ArgumentIsNullOrWhitespace, nameof(key));
        }

        Key = key;
        ConfigurationMetadata = configurationMetadata;
    }

    public string Key { get; }

    public ConfigurationMetadata? ConfigurationMetadata { get; }
}