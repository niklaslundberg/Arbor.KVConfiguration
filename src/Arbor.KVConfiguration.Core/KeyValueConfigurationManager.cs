using System;
using Arbor.KVConfiguration.Core.Decorators;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core
{
    public static class KeyValueConfigurationManager
    {
        public static MultiSourceKeyValueConfiguration Build(
            [NotNull] this AppSettingsBuilder appSettingsBuild,
            Action<string> logAction = null)
        {
            if (appSettingsBuild == null)
            {
                throw new ArgumentNullException(nameof(appSettingsBuild));
            }

            var multiSourceKeyValueConfiguration =
                new MultiSourceKeyValueConfiguration(new DecoratorDelegator(appSettingsBuild), logAction);

            return Initialize(multiSourceKeyValueConfiguration, logAction);
        }

        public static MultiSourceKeyValueConfiguration Build(
            [NotNull] this AppSettingsDecoratorBuilder appSettingsBuild,
            Action<string> logAction = null)
        {
            if (appSettingsBuild == null)
            {
                throw new ArgumentNullException(nameof(appSettingsBuild));
            }

            var multiSourceKeyValueConfiguration = new MultiSourceKeyValueConfiguration(appSettingsBuild, logAction);

            return Initialize(multiSourceKeyValueConfiguration);
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

        private static MultiSourceKeyValueConfiguration Initialize(
            [NotNull] IKeyValueConfiguration keyValueConfiguration,
            Action<string> logAction = null)
        {
            if (keyValueConfiguration == null)
            {
                throw new ArgumentNullException(nameof(keyValueConfiguration));
            }

            if (keyValueConfiguration is MultiSourceKeyValueConfiguration multiSourceKeyValueConfiguration)
            {
                return multiSourceKeyValueConfiguration;
            }

            return
                new MultiSourceKeyValueConfiguration(
                    new DecoratorDelegator(new AppSettingsBuilder(keyValueConfiguration, null)),
                    logAction);
        }
    }
}
