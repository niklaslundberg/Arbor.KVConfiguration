using System.Collections.Immutable;
using Arbor.KVConfiguration.Core;

namespace Arbor.KVConfiguration.Schema
{
    public interface IKeyValueConfigurationWithMetadata : IKeyValueConfiguration
    {
        ImmutableArray<KeyValueConfigurationItem> ConfigurationItems { get; }
    }
}
