using System;
using System.Collections.Generic;
using System.Text;

using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.SystemConfiguration;

namespace Arbor.KVConfiguration.Samples.ConsoleApp
{
    public class AppScenario1
    {
        public void Execute()
        {
            IKeyValueConfiguration appSettingsKeyValueConfiguration = new AppSettingsKeyValueConfiguration();

            KVConfigurationManager.Initialize(appSettingsKeyValueConfiguration);

            var goodKeys = new List<string> {
                                   "a-non-existing-key",
                                   "urn:test:key",
                                   "urn:TEST:key"
                               };

            var keys = Specials.Special;

            foreach (string goodKey in goodKeys)
            {
                keys.Add(goodKey, goodKey);
            }

            var builder = new StringBuilder();

            foreach (var pair in keys)
            {
                builder.AppendLine($"Key: {pair.Key}");

                string value = appSettingsKeyValueConfiguration.ValueOrDefault(pair.Value);

                var displayValue = Specials.GetDisplayValue(value);

                builder.AppendLine($"\t Instance: {displayValue}");

                var staticValue = KVConfigurationManager.AppSettings[pair.Value];

                var staticDisplayValue = Specials.GetDisplayValue(staticValue);

                builder.AppendLine($"\t Static: {staticDisplayValue}");
            }

            Console.WriteLine(builder.ToString());
        }
    }
}
