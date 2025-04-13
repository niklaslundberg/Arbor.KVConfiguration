using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Arbor.KVConfiguration.Microsoft.Extensions.Configuration.Urns;

public sealed class KeyValueConfigurationProvider(KeyValueConfigurationSourceAdapter adapter) : IConfigurationProvider
{
    public bool TryGet(string key, out string? value)
    {
        string? foundValue = adapter.KeyValueConfiguration[key];

        if (string.IsNullOrWhiteSpace(foundValue))
        {
            value = null;
            return false;
        }

        value = foundValue;
        return true;
    }

    public void Set(string key, string? value)
    {
        // Not supported
    }

    public IChangeToken GetReloadToken() => new CancellationChangeToken(CancellationToken.None);

    public void Load()
    {
        // Data is already loaded
    }

    public IEnumerable<string> GetChildKeys(
        IEnumerable<string> earlierKeys,
        string? parentPath)
    {
        string prefix = parentPath is null
            ? string.Empty
            : parentPath + ConfigurationPath.KeyDelimiter;

        return adapter.KeyValueConfiguration.AllValues
            .Where(kv => kv.Key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            .Select(kv => Segment(kv.Key, prefix.Length))
            .Concat(earlierKeys)
            .OrderBy(k => k, ConfigurationKeyComparer.Instance);
    }

    private static string Segment(string key, int prefixLength)
    {
        int indexOf = key.IndexOf(ConfigurationPath.KeyDelimiter, prefixLength, StringComparison.OrdinalIgnoreCase);

        return indexOf < 0
            ? key[prefixLength..]
            : key[prefixLength..indexOf];
    }
}