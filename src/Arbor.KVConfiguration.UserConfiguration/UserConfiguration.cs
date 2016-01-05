using System;
using System.Collections.Generic;
using System.IO;

using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.JsonConfiguration;

namespace Arbor.KVConfiguration.UserConfiguration
{
    public class UserConfiguration : IKeyValueConfiguration
    {
        private readonly IKeyValueConfiguration _configuration;

        public UserConfiguration(IKeyValueConfiguration fallbackConfiguration)
        {
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.user");

            if (File.Exists(file))
            {
                var jsonConfiguration = new JsonKeyValueConfiguration(file);
                _configuration = new ConfigurationWithFallback(jsonConfiguration, fallbackConfiguration);
            }
            else
            {
                _configuration = fallbackConfiguration;
            }
        }

        public IReadOnlyCollection<string> AllKeys => _configuration.AllKeys;

        public IReadOnlyCollection<StringPair> AllValues => _configuration.AllValues;

        public IReadOnlyCollection<MultipleValuesStringPair> AllWithMultipleValues
            => _configuration.AllWithMultipleValues;

        public string this[string key] => _configuration[key];

        public string ValueOrDefault(string key)
        {
            return _configuration.ValueOrDefault(key);
        }

        public string ValueOrDefault(string key, string defaultValue)
        {
            return _configuration.ValueOrDefault(key, defaultValue);
        }
    }
}