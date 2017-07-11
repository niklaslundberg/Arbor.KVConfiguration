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

        public static IKeyValueConfiguration Build(
            [NotNull] this AppSettingsBuilder appSettingsBuild,
            Action<string> logAction = null)
        {
            if (appSettingsBuild == null)
            {
                throw new ArgumentNullException(nameof(appSettingsBuild));
            }
            return new MultiSourceKeyValueConfiguration(new DecoratorDelegate(appSettingsBuild), logAction);
        }

        public static IKeyValueConfiguration Build(
            [NotNull] this AppSettingsDecoratorBuilder appSettingsBuild,
            Action<string> logAction = null)
        {
            if (appSettingsBuild == null)
            {
                throw new ArgumentNullException(nameof(appSettingsBuild));
            }

            return new MultiSourceKeyValueConfiguration(appSettingsBuild, logAction);
        }

        public static AppSettingsBuilder Add([NotNull] IKeyValueConfiguration keyValueConfiguration)
        {
            if (keyValueConfiguration == null)
            {
                throw new ArgumentNullException(nameof(keyValueConfiguration));
            }

            return new AppSettingsBuilder(keyValueConfiguration, null);
        }

        public static AppSettingsBuilder Add(
            [NotNull] this AppSettingsBuilder appSettingsBuilder,
            [NotNull] IKeyValueConfiguration keyValueConfiguration)
        {
            if (appSettingsBuilder == null)
            {
                throw new ArgumentNullException(nameof(appSettingsBuilder));
            }

            if (keyValueConfiguration == null)
            {
                throw new ArgumentNullException(nameof(keyValueConfiguration));
            }

            return new AppSettingsBuilder(keyValueConfiguration, appSettingsBuilder);
        }

        public static AppSettingsDecoratorBuilder DecorateWith(
            [NotNull] this AppSettingsBuilder builder,
            [NotNull] IKeyValueConfigurationDecorator decorator)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (decorator == null)
            {
                throw new ArgumentNullException(nameof(decorator));
            }

            return new AppSettingsDecoratorBuilder(builder, decorator);
        }

        public static AppSettingsDecoratorBuilder DecorateWith(
            [NotNull] this AppSettingsDecoratorBuilder builder,
            [NotNull] IKeyValueConfigurationDecorator decorator)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (decorator == null)
            {
                throw new ArgumentNullException(nameof(decorator));
            }

            return new AppSettingsDecoratorBuilder(builder, decorator);
        }

        public static void Initialize([NotNull] IKeyValueConfiguration keyValueConfiguration)
        {
            if (keyValueConfiguration == null)
            {
                throw new ArgumentNullException(nameof(keyValueConfiguration));
            }

            if (_appSettings != null)
            {
                throw new InvalidOperationException(
                    $"The {nameof(KeyValueConfigurationManager)} is already initialized");
            }

            lock (_MutexLock)
            {
                if (_appSettings != null)
                {
                    throw new InvalidOperationException(
                        $"The {nameof(KeyValueConfigurationManager)} is already initialized");
                }

                _appSettings = keyValueConfiguration;
            }
        }
    }
}
