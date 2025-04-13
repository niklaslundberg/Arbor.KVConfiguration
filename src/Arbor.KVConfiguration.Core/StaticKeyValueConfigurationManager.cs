using System;
using Arbor.KVConfiguration.Core.Extensions;

namespace Arbor.KVConfiguration.Core;

public static class StaticKeyValueConfigurationManager
{
    private static IKeyValueConfiguration? _appSettings;
    private static readonly object MutexLock = new();

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

    public static void Release()
    {
        if (_appSettings is object)
        {
            _appSettings = null;
        }
    }

    public static IKeyValueConfiguration Initialize(IKeyValueConfiguration keyValueConfiguration)
    {
        keyValueConfiguration.ThrowIfNull();

        if (_appSettings is null)
        {
            lock (MutexLock)
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