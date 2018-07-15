using System;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Reflection;
using Arbor.KVConfiguration.Core.Metadata;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core
{
    public sealed class ReflectionKeyValueConfiguration : IKeyValueConfigurationWithMetadata
    {
        private readonly string _assemblyName;
        private readonly ImmutableArray<KeyValueConfigurationItem> _configurationItems;
        private bool _disposed;
        private IKeyValueConfiguration _inMemoryKeyValueConfiguration;

        public ReflectionKeyValueConfiguration([NotNull] Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            _assemblyName = assembly.FullName;

            ImmutableArray<KeyValueConfigurationItem> keyValueConfigurationItems =
                ReflectionConfiguratonReader.ReadConfiguration(assembly);

            var nameValueCollection = new NameValueCollection();

            foreach (KeyValueConfigurationItem keyValueConfigurationItem in keyValueConfigurationItems)
            {
                nameValueCollection.Add(keyValueConfigurationItem.Key, keyValueConfigurationItem.Value);
            }

            _inMemoryKeyValueConfiguration = new InMemoryKeyValueConfiguration(nameValueCollection);
            _configurationItems = keyValueConfigurationItems;
        }

        public ImmutableArray<string> AllKeys
        {
            get
            {
                CheckDisposed();
                return _inMemoryKeyValueConfiguration.AllKeys;
            }
        }

        public ImmutableArray<StringPair> AllValues
        {
            get
            {
                CheckDisposed();
                return _inMemoryKeyValueConfiguration.AllValues;
            }
        }

        public ImmutableArray<MultipleValuesStringPair> AllWithMultipleValues
        {
            get
            {
                CheckDisposed();
                return _inMemoryKeyValueConfiguration.AllWithMultipleValues;
            }
        }

        public string this[string key] => _inMemoryKeyValueConfiguration[key];

        public ImmutableArray<KeyValueConfigurationItem> ConfigurationItems
        {
            get
            {
                CheckDisposed();
                return _configurationItems;
            }
        }

        public void Dispose()
        {
            if (_disposed)
            {
                if (_inMemoryKeyValueConfiguration is IDisposable disposable)
                {
                    disposable.Dispose();
                }

                _inMemoryKeyValueConfiguration = null;
                _disposed = true;
            }
        }

        public override string ToString()
        {
            return $"{base.ToString()} [assembly: '{_assemblyName}']";
        }

        private void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(ToString());
            }
        }
    }
}
