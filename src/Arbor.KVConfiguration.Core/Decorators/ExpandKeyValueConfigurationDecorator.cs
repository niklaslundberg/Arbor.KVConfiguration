using System;
using System.Collections.Immutable;
using System.Linq;

namespace Arbor.KVConfiguration.Core.Decorators
{
    public sealed class ExpandKeyValueConfigurationDecorator : IKeyValueConfigurationDecorator
    {
        private static string ExpandValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            string expanded = Environment.ExpandEnvironmentVariables(value);

            return expanded;
        }

        public ImmutableArray<string> GetAllKeys(IKeyValueConfiguration keyValueConfiguration)
        {
            if (keyValueConfiguration == null)
            {
                throw new ArgumentNullException(nameof(keyValueConfiguration));
            }

            return keyValueConfiguration.AllKeys;
        }

        public string GetValue(string value) => ExpandValue(value);

        public ImmutableArray<StringPair> GetAllValues(IKeyValueConfiguration keyValueConfiguration)
        {
            if (keyValueConfiguration == null)
            {
                throw new ArgumentNullException(nameof(keyValueConfiguration));
            }

            return keyValueConfiguration.AllValues.Select(item => new StringPair(item.Key, ExpandValue(item.Value)))
                .ToImmutableArray();
        }

        public ImmutableArray<MultipleValuesStringPair> GetAllWithMultipleValues(IKeyValueConfiguration keyValueConfiguration)
        {
            if (keyValueConfiguration == null)
            {
                throw new ArgumentNullException(nameof(keyValueConfiguration));
            }

            return keyValueConfiguration.AllWithMultipleValues.Select(item => new MultipleValuesStringPair(item.Key,
                    item.Values.Select(ExpandValue).ToImmutableArray()))
                .ToImmutableArray();
        }
    }
}