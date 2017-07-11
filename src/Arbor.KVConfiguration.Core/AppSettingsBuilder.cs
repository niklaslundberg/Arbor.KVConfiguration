using System;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core
{
    public class AppSettingsBuilder
    {
        public AppSettingsBuilder([NotNull] IKeyValueConfiguration keyValueConfiguration, [CanBeNull] AppSettingsBuilder previous)
        {
            KeyValueConfiguration = keyValueConfiguration ?? throw new ArgumentNullException(nameof(keyValueConfiguration));
            Previous = previous;
        }

        public IKeyValueConfiguration KeyValueConfiguration { get; }
        public AppSettingsBuilder Previous { get; }
    }
}
