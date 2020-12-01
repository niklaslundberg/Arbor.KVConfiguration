using System.Collections.Immutable;
using Arbor.KVConfiguration.Core.Metadata;

namespace Arbor.KVConfiguration.Core
{
    public interface IKeyValueConfigurationWithMetadata : IKeyValueConfiguration
    {
        ImmutableArray<KeyValueConfigurationItem> ConfigurationItems { get; }
    }
}