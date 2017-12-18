using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Linq;
using Arbor.KVConfiguration.Core.Extensions;

namespace Arbor.KVConfiguration.Core
{
    public sealed class InMemoryKeyValueConfiguration : IKeyValueConfiguration
    {
        private readonly string _name;
        private readonly Dictionary<string, ImmutableArray<string>> _keyValueDictionary;

        public InMemoryKeyValueConfiguration(NameValueCollection nameValueCollection) : this(nameValueCollection, string.Empty)
        {
        }

        public InMemoryKeyValueConfiguration(NameValueCollection nameValueCollection, string name)
        {
            if (nameValueCollection == null)
            {
                throw new ArgumentNullException(nameof(nameValueCollection));
            }
            _name = name;

            _keyValueDictionary =
                new Dictionary<string, ImmutableArray<string>>(nameValueCollection.Count + 1,
                    StringComparer.OrdinalIgnoreCase);

            ImmutableArray<string> keys = nameValueCollection.AllKeys.ToImmutableArray();

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

            AllKeys = _keyValueDictionary.Keys.ToImmutableArray();
        }

        public ImmutableArray<string> AllKeys { get; }

        public ImmutableArray<StringPair> AllValues
        {
            get { return AllKeys.Select(key => new StringPair(key, GetCombinedValues(key))).ToImmutableArray(); }
        }

        public ImmutableArray<MultipleValuesStringPair> AllWithMultipleValues
        {
            get
            {
                return
                    AllKeys.Select(key => new MultipleValuesStringPair(key, _keyValueDictionary[key]))
                        .ToImmutableArray();
            }
        }

        public string this[string key] => GetCombinedValues(key);

        private string GetCombinedValues(string key)
        {
            if (key == null)
            {
                return string.Empty;
            }

            if (!_keyValueDictionary.ContainsKey(key))
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

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(_name))
            {
                return $"{base.ToString()} [name: '{_name}']";
            }

            return $"{base.ToString()} [name: 'unnamed']";
        }
    }
}
