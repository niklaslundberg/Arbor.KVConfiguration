using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Linq;
using Arbor.KVConfiguration.Core;
using Microsoft.Configuration.ConfigurationBuilders;

namespace Arbor.KVConfiguration.SystemConfiguration.Providers
{
    public abstract class KeyValueConfigurationBuilder : KeyValueConfigBuilder
    {
        private IKeyValueConfiguration _keyValueConfiguration;

        public override void Initialize(string name, NameValueCollection values)
        {
            base.Initialize(name, values);

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

        public override string GetValue(string key)
        {
            CheckInitialized();

            string value = GetKeyValueConfiguration()[key];

            return value;
        }

        public override ICollection<KeyValuePair<string, string>> GetAllValues(string prefix)
        {
            CheckInitialized();

            if (string.IsNullOrWhiteSpace(prefix))
            {
                ImmutableArray<KeyValuePair<string, string>> values = _keyValueConfiguration.AllWithMultipleValues
                    .SelectMany(pair =>
                        pair.Values.Select(value => new KeyValuePair<string, string>(pair.Key, value)))
                    .ToImmutableArray();

                return values;
            }

            ImmutableArray<KeyValuePair<string, string>> filteredValues = _keyValueConfiguration
                .AllWithMultipleValues
                .Where(item => item.Key.StartsWith(prefix))
                .SelectMany(pair =>
                    pair.Values.Select(value => new KeyValuePair<string, string>(pair.Key, value)))
                .ToImmutableArray();

            return filteredValues;
        }

        protected abstract IKeyValueConfiguration GetKeyValueConfiguration();
    }
}
