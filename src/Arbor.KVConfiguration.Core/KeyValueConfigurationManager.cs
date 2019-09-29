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
            if (appSettingsBuild is null)
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
            if (appSettingsBuild is null)
            {
                throw new ArgumentNullException(nameof(appSettingsBuild));
            }

            var multiSourceKeyValueConfiguration = new MultiSourceKeyValueConfiguration(appSettingsBuild, logAction);

            return Initialize(multiSourceKeyValueConfiguration);
        }

        /// <summary>
        /// Add new configuration, last one wins
        /// </summary>
        /// <param name="keyValueConfiguration"></param>
        /// <returns></returns>
        public static AppSettingsBuilder Add([NotNull] IKeyValueConfiguration keyValueConfiguration)
        {
            if (keyValueConfiguration is null)
            {
                throw new ArgumentNullException(nameof(keyValueConfiguration));
            }

            return new AppSettingsBuilder(keyValueConfiguration, null);
        }

        /// <summary>
        /// Add new configuration, last one wins
        /// </summary>
        /// <param name="appSettingsBuilder"></param>
        /// <param name="keyValueConfiguration"></param>
        /// <returns></returns>
        public static AppSettingsBuilder Add(
            [NotNull] this AppSettingsBuilder appSettingsBuilder,
            [NotNull] IKeyValueConfiguration keyValueConfiguration)
        {
            if (appSettingsBuilder is null)
            {
                throw new ArgumentNullException(nameof(appSettingsBuilder));
            }

            if (keyValueConfiguration is null)
            {
                throw new ArgumentNullException(nameof(keyValueConfiguration));
            }

            return new AppSettingsBuilder(keyValueConfiguration, appSettingsBuilder);
        }

        public static AppSettingsDecoratorBuilder DecorateWith(
            [NotNull] this AppSettingsBuilder builder,
            [NotNull] IKeyValueConfigurationDecorator decorator)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (decorator is null)
            {
                throw new ArgumentNullException(nameof(decorator));
            }

            return new AppSettingsDecoratorBuilder(builder, decorator);
        }

        public static AppSettingsDecoratorBuilder DecorateWith(
            [NotNull] this AppSettingsDecoratorBuilder builder,
            [NotNull] IKeyValueConfigurationDecorator decorator)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (decorator is null)
            {
                throw new ArgumentNullException(nameof(decorator));
            }

            return new AppSettingsDecoratorBuilder(builder, decorator);
        }

        private static MultiSourceKeyValueConfiguration Initialize(
            [NotNull] IKeyValueConfiguration keyValueConfiguration,
            Action<string> logAction = null)
        {
            if (keyValueConfiguration is null)
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
