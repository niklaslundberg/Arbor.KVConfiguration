using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Linq;
using Arbor.KVConfiguration.Core.Extensions;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core
{
    public sealed class InMemoryKeyValueConfiguration : IKeyValueConfiguration
    {
        private readonly string _name;
        private ImmutableArray<string> _allKeys;
        private bool _disposed;
        private Dictionary<string, ImmutableArray<string>>? _keyValueDictionary;

        public InMemoryKeyValueConfiguration(NameValueCollection nameValueCollection) : this(nameValueCollection,
            string.Empty)
        {
        }

        [PublicAPI]
        public InMemoryKeyValueConfiguration(NameValueCollection nameValueCollection, string name)
        {
            if (nameValueCollection is null)
            {
                throw new ArgumentNullException(nameof(nameValueCollection));
            }

            _name = name ?? string.Empty;

            _keyValueDictionary =
                new Dictionary<string, ImmutableArray<string>>(nameValueCollection.Count + 1,
                    StringComparer.OrdinalIgnoreCase);

            var keys = nameValueCollection.AllKeys.ToImmutableArray();

            foreach (string key in keys)
            {
                ImmutableArray<string> values = nameValueCollection.GetValues(key).SafeToImmutableArray();

                if (!string.IsNullOrWhiteSpace(key))
                {
                    if (!_keyValueDictionary.ContainsKey(key))
                    {
                        _keyValueDictionary.Add(key, values);
                    }
                    else
                    {
                        _keyValueDictionary[key] = _keyValueDictionary[key].AddRange(values);
                    }
                }
            }

            _allKeys = _keyValueDictionary.Keys.ToImmutableArray();
        }

        public ImmutableArray<string> AllKeys
        {
            get
            {
                CheckDisposed();

                return _allKeys;
            }
        }

        public ImmutableArray<StringPair> AllValues
        {
            get
            {
                CheckDisposed();

                return AllKeys.Select(key => new StringPair(key, GetCombinedValues(key))).ToImmutableArray();
            }
        }

        public ImmutableArray<MultipleValuesStringPair> AllWithMultipleValues
        {
            get
            {
                CheckDisposed();

                return AllKeys
                    .Select(key => new MultipleValuesStringPair(key, _keyValueDictionary![key]))
                    .ToImmutableArray();
            }
        }

        public string this[string key] => GetCombinedValues(key);

        private string GetCombinedValues(string key)
        {
            CheckDisposed();

            if (key is null)
            {
                return string.Empty;
            }

            if (!_keyValueDictionary!.ContainsKey(key))
            {
                return string.Empty;
            }

            ImmutableArray<string> values = _keyValueDictionary[key];

            if (values.IsEmpty)
            {
                return string.Empty;
            }

            if (values.Length == 1)
            {
                return values[0];
            }

            return string.Join(",", values);
        }

        private void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(ToString());
            }
        }

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(_name))
            {
                return $"{base.ToString()} [name: '{_name}']";
            }

            return $"{base.ToString()} [name: 'unnamed']";
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _keyValueDictionary?.Clear();
                _keyValueDictionary = null;
                _allKeys = default;
                _disposed = true;
            }
        }
    }
}