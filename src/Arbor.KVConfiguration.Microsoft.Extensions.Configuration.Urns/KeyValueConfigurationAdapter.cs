using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Linq;
using Arbor.KVConfiguration.Core;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;

namespace Arbor.KVConfiguration.Microsoft.Extensions.Configuration.Urns
{
    /// <summary>
    /// Adapter to use an existing IConfiguration with Arbor.KVConfiguration
    /// </summary>
    public sealed class KeyValueConfigurationAdapter : IKeyValueConfiguration, IDisposable
    {
        private InMemoryKeyValueConfiguration _inMemoryConfig;
        private bool _isDisposed;

        public KeyValueConfigurationAdapter([NotNull] IConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var nameValueCollection = new NameValueCollection();

            foreach (KeyValuePair<string, string> configurationSection in config.AsEnumerable()
                .Where(pair => !string.IsNullOrWhiteSpace(pair.Key)
                               && !string.IsNullOrWhiteSpace(pair.Value)))
            {
                nameValueCollection.Add(configurationSection.Key, configurationSection.Value);
            }

            _inMemoryConfig = new InMemoryKeyValueConfiguration(nameValueCollection);
        }

        public ImmutableArray<string> AllKeys
        {
            get
            {
                CheckDisposed();
                return _inMemoryConfig.AllKeys;
            }
        }

        public ImmutableArray<StringPair> AllValues
        {
            get
            {
                CheckDisposed();
                return _inMemoryConfig.AllValues;
            }
        }

        public ImmutableArray<MultipleValuesStringPair> AllWithMultipleValues
        {
            get
            {
                CheckDisposed();
                return _inMemoryConfig.AllWithMultipleValues;
            }
        }

        public string this[string key]
        {
            get
            {
                CheckDisposed();
                return _inMemoryConfig[key];
            }
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _inMemoryConfig?.Dispose();
                _inMemoryConfig = null;
                _isDisposed = true;
            }
        }

        private void CheckDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }
    }
}
