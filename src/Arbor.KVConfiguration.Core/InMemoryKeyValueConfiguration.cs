using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Arbor.KVConfiguration.Core
{
    public class InMemoryKeyValueConfiguration : IKeyValueConfiguration
    {
        private readonly NameValueCollection _nameValueCollection;

        public InMemoryKeyValueConfiguration(NameValueCollection nameValueCollection)
        {
            if (nameValueCollection == null)
            {
                throw new ArgumentNullException(nameof(nameValueCollection));
            }

            var copy = new NameValueCollection(nameValueCollection.Count + 1) { nameValueCollection };

            _nameValueCollection = copy;
        }

        public IReadOnlyCollection<string> AllKeys => _nameValueCollection.AllKeys;

        public IReadOnlyCollection<StringPair> AllValues
        {
            get
            {
                return AllKeys.Select(key => new StringPair(key, _nameValueCollection.Get(key))).ToArray();
            }
        }

        public IReadOnlyCollection<MultipleValuesStringPair> AllWithMultipleValues
        {
            get
            {
                return
                    AllKeys.Select(key => new MultipleValuesStringPair(key, _nameValueCollection.GetValues(key)))
                        .ToArray();
            }
        }

        public string this[string key] => _nameValueCollection[key];

        public string ValueOrDefault(string key)
        {
            string value = _nameValueCollection[key];

            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            return value;
        }

        public string ValueOrDefault(string key, string defaultValue)
        {
            string value = _nameValueCollection[key];

            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }

            return value;
        }
    }
}