using System;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core
{
    public sealed class ConfigurationWithFallback : IKeyValueConfiguration
    {
        private readonly IKeyValueConfiguration _fallbackConfiguration;

        private readonly IKeyValueConfiguration _primaryConfiguration;

        public ConfigurationWithFallback(
            [NotNull] IKeyValueConfiguration primaryConfiguration,
            [NotNull] IKeyValueConfiguration fallbackConfiguration)
        {
            _primaryConfiguration = primaryConfiguration ??
                                    throw new ArgumentNullException(nameof(primaryConfiguration));
            _fallbackConfiguration = fallbackConfiguration ??
                                     throw new ArgumentNullException(nameof(fallbackConfiguration));
        }

        public ImmutableArray<string> AllKeys
            => _primaryConfiguration.AllKeys.Union(_fallbackConfiguration.AllKeys).Distinct().ToImmutableArray();

        public ImmutableArray<StringPair> AllValues => AllKeys.Select(key => new StringPair(key, GetValue(key)))
            .ToImmutableArray();

        public ImmutableArray<MultipleValuesStringPair> AllWithMultipleValues
        {
            get
            {
                ImmutableArray<MultipleValuesStringPair> fallbackOnly =
                    _fallbackConfiguration.AllWithMultipleValues
                        .Where(pair =>
                            !_primaryConfiguration.AllKeys.Contains(pair.Key, StringComparer.OrdinalIgnoreCase))
                        .ToImmutableArray();

                return _primaryConfiguration.AllWithMultipleValues.Concat(fallbackOnly).ToImmutableArray();
            }
        }

        public string this[string key] => GetValue(key);

        private string GetValue(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }

            string value = _primaryConfiguration[key];

            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            return _fallbackConfiguration[key];
        }
    }
}
