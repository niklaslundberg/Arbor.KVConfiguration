using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Linq;

namespace Arbor.KVConfiguration.Core
{
    public class InMemoryKeyValueConfiguration : IKeyValueConfiguration
    {
        private readonly ImmutableDictionary<string, ImmutableArray<string>> _nameValueCollection;

        public InMemoryKeyValueConfiguration(NameValueCollection nameValueCollection)
        {
            if (nameValueCollection == null)
            {
                throw new ArgumentNullException(nameof(nameValueCollection));
            }

            var tempDictionary = new Dictionary<string, ImmutableArray<string>>(StringComparer.OrdinalIgnoreCase);

            ImmutableList<string> keys = nameValueCollection.AllKeys.ToImmutableList();

            foreach (string key in keys)
            {
                ImmutableArray<string> values = nameValueCollection.GetValues(key)?.ToImmutableArray() ?? ImmutableArray<string>.Empty;

                if (!string.IsNullOrWhiteSpace(key))
                {
                    if (!tempDictionary.ContainsKey(key))
                    {
                        tempDictionary.Add(key, values);
                    }
                    else
                    {
                        tempDictionary[key] = tempDictionary[key].AddRange(values);
                    }
                }
            }

            _nameValueCollection = tempDictionary.ToImmutableDictionary(
                keyValuePair => keyValuePair.Key,
                keyValuePair => keyValuePair.Value,
                StringComparer.OrdinalIgnoreCase);
        }

        public ImmutableArray<string> AllKeys => _nameValueCollection.Keys.ToImmutableArray();

        public ImmutableArray<StringPair> AllValues
        {
            get
            {
                return AllKeys.Select(key => new StringPair(key, GetCombinedValues(key))).ToImmutableArray();
            }
        }

        public ImmutableArray<MultipleValuesStringPair> AllWithMultipleValues
        {
            get
            {
                return
                    AllKeys.Select(key => new MultipleValuesStringPair(key, _nameValueCollection[key]))
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

            if (!_nameValueCollection.ContainsKey(key))
            {
                return string.Empty;
            }

            ImmutableArray<string> values = _nameValueCollection[key];

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
    }
}
