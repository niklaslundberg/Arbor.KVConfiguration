using System;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core
{
    public sealed class AppSettingsBuilder : IDisposable
    {
        public AppSettingsBuilder(
            [NotNull] IKeyValueConfiguration keyValueConfiguration,
            [CanBeNull] AppSettingsBuilder previous)
        {
            KeyValueConfiguration = keyValueConfiguration ??
                                    throw new ArgumentNullException(nameof(keyValueConfiguration));
            Previous = previous;
        }

        public IKeyValueConfiguration KeyValueConfiguration { get; }

        public AppSettingsBuilder Previous { get; }

        public void Dispose()
        {
            Previous?.Dispose();

            if (KeyValueConfiguration is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
