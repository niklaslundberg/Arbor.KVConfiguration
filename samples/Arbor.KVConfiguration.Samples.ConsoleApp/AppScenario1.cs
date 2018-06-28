using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Core.Extensions.StringExtensions;
using Arbor.KVConfiguration.SystemConfiguration;

namespace Arbor.KVConfiguration.Samples.ConsoleApp
{
    public static class AppScenario1
    {
        public static void Execute()
        {
            IKeyValueConfiguration appSettingsKeyValueConfiguration = new AppSettingsKeyValueConfiguration();

            KeyValueConfigurationManager.Add(appSettingsKeyValueConfiguration).Build();

            var goodKeys = new List<string>
            {
                "a-non-existing-key",
                "urn:test:key",
                "urn:TEST:key"
            };

            Dictionary<string, string> keys = Specials.Special;

            foreach (string goodKey in goodKeys)
            {
                keys.Add(goodKey, goodKey);
            }

            var builder = new StringBuilder();

            foreach (KeyValuePair<string, string> pair in keys)
            {
                builder.AppendFormat(CultureInfo.InvariantCulture, "Key: {0}", pair.Key).AppendLine();

                string value = appSettingsKeyValueConfiguration.ValueOrDefault(pair.Value);

                string displayValue = Specials.GetDisplayValue(value);

                builder.Append("\t Instance: ").AppendLine(displayValue);

                string staticValue = StaticKeyValueConfigurationManager.AppSettings[pair.Value];

                string staticDisplayValue = Specials.GetDisplayValue(staticValue);

                builder.Append("\t Static: ").AppendLine(staticDisplayValue);
            }

            Console.WriteLine(builder.ToString());
        }
    }
}
