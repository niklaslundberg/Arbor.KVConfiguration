using System;
using System.Collections.Generic;

namespace Arbor.KVConfiguration.Schema.Json
{
    public class Configuration
    {
        public string Version { get; }

        public IReadOnlyCollection<KeyValue> Keys { get; }

        public Configuration(string version, IReadOnlyCollection<KeyValue> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }
            if (string.IsNullOrWhiteSpace(version))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(version));
            }
            Version = version;
            Keys = keys;
        }
    }
}
