using System;

using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core
{
    public static class KVConfigurationManager
    {
        private static readonly object _MutexLock = new object();

        private static IKeyValueConfiguration appSettings;

        public static IKeyValueConfiguration AppSettings
        {
            get
            {
                if (appSettings == null)
                {
                    throw new InvalidOperationException(
                        $"The {nameof(KVConfigurationManager)} has not been initialized");
                }

                return appSettings;
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
                appSettings = keyValueConfiguration;
            }
        }
    }
}