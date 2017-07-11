using System;
using System.Collections.Immutable;
using System.Linq;

namespace Arbor.KVConfiguration.Core
{
    public sealed class ExpandKeyValueConfigurationDecorator : IKeyValueConfigurationDecorator
    {
        public ImmutableArray<string> GetAllKeys(IKeyValueConfiguration keyValueConfiguration)
        {
            return keyValueConfiguration.AllKeys;
        }

        public string GetValue(string value)
        {
            return ExpandValue(value);
        }

        public ImmutableArray<StringPair> GetAllValues(IKeyValueConfiguration keyValueConfiguration)
        {
            return keyValueConfiguration.AllValues
                .Select(item => new StringPair(item.Key, ExpandValue(item.Value)))
                .ToImmutableArray();
        }

        public ImmutableArray<MultipleValuesStringPair> GetAllWithMultipleValues(
            IKeyValueConfiguration keyValueConfiguration)
        {
            return keyValueConfiguration.AllWithMultipleValues
                .Select(item => new MultipleValuesStringPair(item.Key,
                    item.Values.Select(ExpandValue).ToImmutableArray()))
                .ToImmutableArray();
        }

        private string ExpandValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            return Environment.ExpandEnvironmentVariables(value);
        }
    }
}
