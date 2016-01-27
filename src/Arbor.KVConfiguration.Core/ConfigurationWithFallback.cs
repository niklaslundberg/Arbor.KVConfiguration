using System;
using System.Collections.Generic;
using System.Linq;

namespace Arbor.KVConfiguration.Core
{
    public class ConfigurationWithFallback : IKeyValueConfiguration
    {
        private readonly IKeyValueConfiguration _fallbackConfiguration;

        private readonly IKeyValueConfiguration _primayConfiguration;

        public ConfigurationWithFallback(
            IKeyValueConfiguration primayConfiguration,
            IKeyValueConfiguration fallbackConfiguration)
        {
            _primayConfiguration = primayConfiguration;
            _fallbackConfiguration = fallbackConfiguration;
        }

        public IReadOnlyCollection<string> AllKeys
            => _primayConfiguration.AllKeys.Union(_fallbackConfiguration.AllKeys).Distinct().ToArray();

        public IReadOnlyCollection<StringPair> AllValues
        {
            get
            {
                return AllKeys.Select(key => new StringPair(key, ValueOrDefault(key))).ToList();
            }
        }

        public IReadOnlyCollection<MultipleValuesStringPair> AllWithMultipleValues
        {
            get
            {
                var fallbackOnly =
                    _fallbackConfiguration.AllWithMultipleValues.Where(
                        pair =>
                        !_primayConfiguration.AllKeys.Contains(pair.Key, StringComparer.InvariantCultureIgnoreCase));

                return _primayConfiguration.AllWithMultipleValues.Concat(fallbackOnly).ToArray();
            }
        }

        public string this[string key] => ValueOrDefault(key);

        public string ValueOrDefault(string key)
        {
            var value = _primayConfiguration.ValueOrDefault(key);

            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            return _fallbackConfiguration.ValueOrDefault(key);
        }

        public string ValueOrDefault(string key, string defaultValue)
        {
            var value = _primayConfiguration.ValueOrDefault(key, defaultValue);

            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            return _fallbackConfiguration.ValueOrDefault(key, defaultValue);
        }
    }
}
