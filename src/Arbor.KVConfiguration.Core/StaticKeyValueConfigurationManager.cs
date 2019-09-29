using System;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core
{
    public static class StaticKeyValueConfigurationManager
    {
        private static IKeyValueConfiguration _appSettings;
        private static readonly object _MutexLock = new object();

        public static void Release()
        {
            if (_appSettings is null)
            {
                _appSettings = null;
            }
        }

        public static IKeyValueConfiguration AppSettings
        {
            get
            {
                if (_appSettings is null)
                {
                    throw new InvalidOperationException(
                        $"The {nameof(StaticKeyValueConfigurationManager)} has not yet been initialized, please ensure to call {nameof(Initialize)} method first");
                }

                return _appSettings;
            }
        }

        public static bool IsInitialized => _appSettings is object;

        public static IKeyValueConfiguration Initialize([NotNull] IKeyValueConfiguration keyValueConfiguration)
        {
            if (keyValueConfiguration is null)
            {
                throw new ArgumentNullException(nameof(keyValueConfiguration));
            }

            if (_appSettings is null)
            {
                lock (_MutexLock)
                {
                    if (_appSettings is null)
                    {
                        _appSettings = keyValueConfiguration;
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            $"The {nameof(StaticKeyValueConfigurationManager)} has already been initialized");
                    }
                }
            }

            return _appSettings;
        }
    }
}
