using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Arbor.KVConfiguration.Core;

namespace Arbor.KVConfiguration.SystemConfiguration.Providers
{
    public abstract class KeyValueConfigurationBuilder : KeyValueConfigBuilder
    {
        private IKeyValueConfiguration? _keyValueConfiguration;

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);

            _keyValueConfiguration = GetKeyValueConfiguration();

            CheckInitialized();
        }

        private void CheckInitialized()
        {
            if (_keyValueConfiguration is null)
            {
                throw new InvalidOperationException($"The {nameof(IKeyValueConfiguration)} has not been initialized");
            }
        }

        public override string? GetValue(string key)
        {
            CheckInitialized();

            string? value = GetKeyValueConfiguration()[key];

            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return value;
        }

        public override ICollection<KeyValuePair<string, string>> GetAllValues(string prefix)
        {
            CheckInitialized();

            if (string.IsNullOrWhiteSpace(prefix))
            {
                var values = _keyValueConfiguration!.AllWithMultipleValues
                    .SelectMany(pair =>
                        pair.Values.Select(value => new KeyValuePair<string, string>(pair.Key, value)))
                    .ToImmutableArray();

                return values;
            }

            var filteredValues = _keyValueConfiguration!
                .AllWithMultipleValues
                .Where(item => item.Key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                .SelectMany(pair =>
                    pair.Values.Select(value => new KeyValuePair<string, string>(pair.Key, value)))
                .ToImmutableArray();

            return filteredValues;
        }

        protected abstract IKeyValueConfiguration GetKeyValueConfiguration();
    }
}