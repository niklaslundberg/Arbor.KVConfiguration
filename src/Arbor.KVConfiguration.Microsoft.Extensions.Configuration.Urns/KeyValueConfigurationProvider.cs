using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Arbor.KVConfiguration.Microsoft.Extensions.Configuration.Urns
{
    public sealed class KeyValueConfigurationProvider : IConfigurationProvider
    {
        private readonly KeyValueConfigurationSourceAdapter _adapter;

        public KeyValueConfigurationProvider(KeyValueConfigurationSourceAdapter adapter) => _adapter = adapter;

        public bool TryGet(string key, out string value)
        {
            string foundValue = _adapter.KeyValueConfiguration[key];

            if (string.IsNullOrWhiteSpace(foundValue))
            {
                value = default;
                return false;
            }

            value = foundValue;
            return true;
        }

        public void Set(string key, string value)
        {
            // Not supported
        }

        public IChangeToken GetReloadToken() => new CancellationChangeToken(default);

        public void Load()
        {
            // Data is already loaded
        }

        public IEnumerable<string> GetChildKeys(
            IEnumerable<string> earlierKeys,
            string parentPath)
        {
            string prefix = parentPath == null ? string.Empty : parentPath + ConfigurationPath.KeyDelimiter;

            return _adapter.KeyValueConfiguration.AllValues
                .Where(kv => kv.Key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                .Select(kv => Segment(kv.Key, prefix.Length))
                .Concat(earlierKeys)
                .OrderBy(k => k, ConfigurationKeyComparer.Instance);
        }

        private static string Segment(string key, int prefixLength)
        {
            int indexOf = key.IndexOf(ConfigurationPath.KeyDelimiter, prefixLength, StringComparison.OrdinalIgnoreCase);
            return indexOf < 0 ? key.Substring(prefixLength) : key.Substring(prefixLength, indexOf - prefixLength);
        }
    }
}
