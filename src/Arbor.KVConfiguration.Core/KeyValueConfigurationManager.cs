using System;
using Arbor.KVConfiguration.Core.Decorators;
using Arbor.KVConfiguration.Core.Extensions;

namespace Arbor.KVConfiguration.Core;

public static class KeyValueConfigurationManager
{
    public static MultiSourceKeyValueConfiguration Build(
        this AppSettingsBuilder appSettingsBuild,
        Action<string>? logAction = null)
    {
        appSettingsBuild.ThrowIfNull();

        var multiSourceKeyValueConfiguration =
            new MultiSourceKeyValueConfiguration(new DecoratorDelegator(appSettingsBuild), logAction);

        return Initialize(multiSourceKeyValueConfiguration, logAction);
    }

    public static MultiSourceKeyValueConfiguration Build(
        this AppSettingsDecoratorBuilder appSettingsBuild,
        Action<string>? logAction = null)
    {
        appSettingsBuild.ThrowIfNull();

        var multiSourceKeyValueConfiguration = new MultiSourceKeyValueConfiguration(appSettingsBuild, logAction);

        return Initialize(multiSourceKeyValueConfiguration);
    }

    /// <summary>
    ///     Add new configuration, last one wins
    /// </summary>
    /// <param name="keyValueConfiguration"></param>
    /// <returns></returns>
    public static AppSettingsBuilder Add(IKeyValueConfiguration keyValueConfiguration)
    {
        keyValueConfiguration.ThrowIfNull();

        return new AppSettingsBuilder(keyValueConfiguration, null);
    }

    /// <summary>
    ///     Add new configuration, last one wins
    /// </summary>
    /// <param name="appSettingsBuilder"></param>
    /// <param name="keyValueConfiguration"></param>
    /// <returns></returns>
    public static AppSettingsBuilder Add(
        this AppSettingsBuilder appSettingsBuilder,
        IKeyValueConfiguration keyValueConfiguration)
    {
        appSettingsBuilder.ThrowIfNull();

        keyValueConfiguration.ThrowIfNull();

        return new AppSettingsBuilder(keyValueConfiguration, appSettingsBuilder);
    }

    public static AppSettingsDecoratorBuilder DecorateWith(
        this AppSettingsBuilder builder,
        IKeyValueConfigurationDecorator decorator)
    {
        builder.ThrowIfNull();

        decorator.ThrowIfNull();

        return new AppSettingsDecoratorBuilder(builder, decorator);
    }

    public static AppSettingsDecoratorBuilder DecorateWith(
        this AppSettingsDecoratorBuilder builder,
        IKeyValueConfigurationDecorator decorator)
    {
        builder.ThrowIfNull();

        decorator.ThrowIfNull();

        return new AppSettingsDecoratorBuilder(builder, decorator);
    }

    private static MultiSourceKeyValueConfiguration Initialize(
        IKeyValueConfiguration keyValueConfiguration,
        Action<string>? logAction = null)
    {
        keyValueConfiguration.ThrowIfNull();

        logAction ??= s => { };

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