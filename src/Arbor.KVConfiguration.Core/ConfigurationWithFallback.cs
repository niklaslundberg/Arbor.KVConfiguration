using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core
{
    public class ConfigurationWithFallback : IKeyValueConfiguration
    {
        private readonly IKeyValueConfiguration _fallbackConfiguration;

        private readonly IKeyValueConfiguration _primayConfiguration;

        public ConfigurationWithFallback(
            [NotNull] IKeyValueConfiguration primayConfiguration,
            [NotNull] IKeyValueConfiguration fallbackConfiguration)
        {
            if (primayConfiguration == null)
            {
                throw new ArgumentNullException(nameof(primayConfiguration));
            }

            if (fallbackConfiguration == null)
            {
                throw new ArgumentNullException(nameof(fallbackConfiguration));
            }

            _primayConfiguration = primayConfiguration;
            _fallbackConfiguration = fallbackConfiguration;
        }

        public IReadOnlyCollection<string> AllKeys
            => _primayConfiguration.AllKeys.Union(_fallbackConfiguration.AllKeys).Distinct().ToArray();

        public IReadOnlyCollection<StringPair> AllValues
        {
            get
            {
                return AllKeys.Select(key => new StringPair(key, GetValue(key))).ToList();
            }
        }

        public IReadOnlyCollection<MultipleValuesStringPair> AllWithMultipleValues
        {
            get
            {
                var fallbackOnly =
                    _fallbackConfiguration.AllWithMultipleValues.Where(
                        pair =>
                        !_primayConfiguration.AllKeys.Contains(pair.Key, StringComparer.OrdinalIgnoreCase));

                return _primayConfiguration.AllWithMultipleValues.Concat(fallbackOnly).ToArray();
            }
        }

        public string this[string key] => GetValue(key);

        private string GetValue(string key)
        {
            var value = _primayConfiguration[key];

            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            return _fallbackConfiguration[key];
        }
    }
}
