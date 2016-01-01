using System.Configuration;

using Arbor.KVConfiguration.Core;

namespace Arbor.KVConfiguration.SystemConfiguration
{
    public class AppSettingsKeyValueConfiguration : IKeyValueConfiguration
    {
        public string this[string key] => ConfigurationManager.AppSettings.Get(key);

        public string ValueOrDefault(string key)
        {
            return ValueOrDefault(key, "");
        }

        public string ValueOrDefault(string key, string defaultValue)
        {
            string value = this[key];

            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }

            return value;
        }
    }
}
