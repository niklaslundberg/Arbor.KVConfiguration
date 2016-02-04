using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Linq;

namespace Arbor.KVConfiguration.Core
{
    public class InMemoryKeyValueConfiguration : IKeyValueConfiguration
    {
        private readonly ImmutableDictionary<string, ImmutableList<string>> _nameValueCollection;

        public InMemoryKeyValueConfiguration(NameValueCollection nameValueCollection)
        {
            if (nameValueCollection == null)
            {
                throw new ArgumentNullException(nameof(nameValueCollection));
            }

            var tempDictionary = new Dictionary<string, ImmutableList<string>>(StringComparer.OrdinalIgnoreCase);

            ImmutableList<string> keys = nameValueCollection.AllKeys.ToImmutableList();

            foreach (string key in keys)
            {
                List<string> values = (nameValueCollection.GetValues(key) ?? Enumerable.Empty<string>()).ToList();

                if (!string.IsNullOrWhiteSpace(key))
                {
                    if (!tempDictionary.ContainsKey(key))
                    {
                        tempDictionary.Add(key, values.ToImmutableList());
                    }
                    else
                    {
                        tempDictionary[key] = tempDictionary[key].AddRange(values);
                    }
                }
            }

            _nameValueCollection = tempDictionary.ToImmutableDictionary(keyValuePair => keyValuePair.Key, keyValuePair=> keyValuePair.Value, StringComparer.OrdinalIgnoreCase);
        }

        public IReadOnlyCollection<string> AllKeys => _nameValueCollection.Keys.ToImmutableList();

        public IReadOnlyCollection<StringPair> AllValues
        {
            get
            {
                return AllKeys.Select(key => new StringPair(key, GetCombinedValues(key))).ToImmutableList();
            }
        }

        private string GetCombinedValues(string key)
        {
            if (key == null)
            {
                return string.Empty;
            }

            if (!_nameValueCollection.ContainsKey(key))
            {
                return string.Empty;
            }

            var values = _nameValueCollection[key];
            if (values.IsEmpty)
            {
                return string.Empty;
            }

            if (values.Count == 1)
            {
                return values[0];
            }

            return string.Join(",", values);
        }

        public IReadOnlyCollection<MultipleValuesStringPair> AllWithMultipleValues
        {
            get
            {
                return
                    AllKeys.Select(key => new MultipleValuesStringPair(key, _nameValueCollection[key]))
                        .ToImmutableList();
            }
        }

        public string this[string key] => GetCombinedValues(key);

        public string ValueOrDefault(string key)
        {
            string value = GetCombinedValues(key);

            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            return value;
        }

        public string ValueOrDefault(string key, string defaultValue)
        {
            string value = GetCombinedValues(key);

            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }

            return value;
        }
    }
}
