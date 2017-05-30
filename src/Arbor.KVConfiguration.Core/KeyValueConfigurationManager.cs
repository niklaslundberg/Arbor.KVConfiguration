using System;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core
{
    public static class KeyValueConfigurationManager
    {
        private static readonly object _MutexLock = new object();

        private static IKeyValueConfiguration _appSettings;

        public static IKeyValueConfiguration AppSettings
        {
            get
            {
                if (_appSettings == null)
                {
                    throw new InvalidOperationException(
                        $"The {nameof(KeyValueConfigurationManager)} has not been initialized");
                }

                return _appSettings;
            }
        }

        public static void Initialize([NotNull] IKeyValueConfiguration keyValueConfiguration)
        {
            if (keyValueConfiguration == null)
            {
                throw new ArgumentNullException(nameof(keyValueConfiguration));
            }

            lock (_MutexLock)
            {
                _appSettings = keyValueConfiguration;
            }
        }
    }
}
