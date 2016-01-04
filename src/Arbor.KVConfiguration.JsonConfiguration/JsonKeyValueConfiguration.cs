namespace Arbor.KVConfiguration.JsonConfiguration
{
    using System;
    using System.Collections.Generic;

    using Arbor.KVConfiguration.Core;

    public class JsonKeyValueConfiguration : IKeyValueConfiguration
    {
        private readonly string _fileFullPath;

        private IKeyValueConfiguration _inMemoryKeyValueConfiguration;

        public JsonKeyValueConfiguration(string fileFullPath)
        {
            _fileFullPath = fileFullPath;
        }


        public IReadOnlyCollection<string> AllKeys => _inMemoryKeyValueConfiguration.AllKeys;

        public IReadOnlyCollection<KeyValuePair<string, string>> AllValues => _inMemoryKeyValueConfiguration.AllValues;

        public IReadOnlyCollection<KeyValuePair<string, IReadOnlyCollection<string>>> AllWithMultipleValues
            => _inMemoryKeyValueConfiguration.AllWithMultipleValues;

        public string this[string key] => _inMemoryKeyValueConfiguration[key];

        public string ValueOrDefault(string key)
        {
            return _inMemoryKeyValueConfiguration.ValueOrDefault(key);
        }

        public string ValueOrDefault(string key, string defaultValue)
        {
            return _inMemoryKeyValueConfiguration.ValueOrDefault(defaultValue);
        }
    }
}