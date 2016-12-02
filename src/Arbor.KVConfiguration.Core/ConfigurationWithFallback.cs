using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core
{
    public class ConfigurationWithFallback : IKeyValueConfiguration
    {
        private readonly IKeyValueConfiguration _fallbackConfiguration;

        private readonly IKeyValueConfiguration _primaryConfiguration;

        public ConfigurationWithFallback(
            [NotNull] IKeyValueConfiguration primaryConfiguration,
            [NotNull] IKeyValueConfiguration fallbackConfiguration)
        {
            if (primaryConfiguration == null)
            {
                throw new ArgumentNullException(nameof(primaryConfiguration));
            }

            if (fallbackConfiguration == null)
            {
                throw new ArgumentNullException(nameof(fallbackConfiguration));
            }

            _primaryConfiguration = primaryConfiguration;
            _fallbackConfiguration = fallbackConfiguration;
        }

        public IReadOnlyCollection<string> AllKeys
            => _primaryConfiguration.AllKeys.Union(_fallbackConfiguration.AllKeys).Distinct().ToImmutableArray();

        public IReadOnlyCollection<StringPair> AllValues
        {
            get { return AllKeys.Select(key => new StringPair(key, GetValue(key))).ToImmutableArray(); }
        }

        public IReadOnlyCollection<MultipleValuesStringPair> AllWithMultipleValues
        {
            get
            {
                IEnumerable<MultipleValuesStringPair> fallbackOnly =
                    _fallbackConfiguration.AllWithMultipleValues
                        .Where(pair =>
                                !_primaryConfiguration.AllKeys.Contains(pair.Key, StringComparer.OrdinalIgnoreCase));

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
