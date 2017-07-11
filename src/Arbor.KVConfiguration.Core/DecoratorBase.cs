using System;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core
{
    public abstract class DecoratorBase : IKeyValueConfigurationDecorator
    {
        public ImmutableArray<string> GetAllKeys([NotNull] IKeyValueConfiguration keyValueConfiguration)
        {
            if (keyValueConfiguration == null)
            {
                throw new ArgumentNullException(nameof(keyValueConfiguration));
            }

            return keyValueConfiguration.AllKeys;
        }

        public abstract string GetValue(string value);

        public ImmutableArray<StringPair> GetAllValues(IKeyValueConfiguration keyValueConfiguration)
        {
            if (keyValueConfiguration == null)
            {
                throw new ArgumentNullException(nameof(keyValueConfiguration));
            }

            return ImmutableArrayExtensions.Select(keyValueConfiguration.AllValues, item => new StringPair(item.Key, GetValue(item.Value)))
                .ToImmutableArray();
        }

        public ImmutableArray<MultipleValuesStringPair> GetAllWithMultipleValues(
            IKeyValueConfiguration keyValueConfiguration)
        {
            if (keyValueConfiguration == null)
            {
                throw new ArgumentNullException(nameof(keyValueConfiguration));
            }

            return keyValueConfiguration.AllWithMultipleValues.Select(item => new MultipleValuesStringPair(item.Key,
                    item.Values.Select<string, string>(GetValue).ToImmutableArray()))
                .ToImmutableArray();
        }
    }
}
