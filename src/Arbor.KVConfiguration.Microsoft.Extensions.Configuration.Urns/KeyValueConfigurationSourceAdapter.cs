using System;
using Arbor.KVConfiguration.Core;

namespace Arbor.KVConfiguration.Microsoft.Extensions.Configuration.Urns
{
    public class KeyValueConfigurationSourceAdapter : IConfigurationSource
    {
        public KeyValueConfigurationSourceAdapter([NotNull] IKeyValueConfiguration keyValueConfiguration) =>
            KeyValueConfiguration =
                keyValueConfiguration ?? throw new ArgumentNullException(nameof(keyValueConfiguration));

        public IKeyValueConfiguration KeyValueConfiguration { get; }

        public IConfigurationProvider Build(IConfigurationBuilder builder) => new KeyValueConfigurationProvider(this);
    }
}