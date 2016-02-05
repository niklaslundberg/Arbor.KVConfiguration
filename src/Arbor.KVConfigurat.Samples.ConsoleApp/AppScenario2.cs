using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

using Arbor.KVConfiguration.Core;

namespace Arbor.KVConfigurat.Samples.ConsoleApp
{
    public class AppScenario2
    {
        public void Execute()
        {
            var collection = new NameValueCollection
                                 {
                                     { string.Empty, string.Empty },
                                     { null, null },
                                     { null, string.Empty },
                                     { string.Empty, null },
                                     { "\t", "\t" },
                                     { "urn:test:key", "a-test-value" },
                                     { "urn:test:KEY", "second-test-value" },
                                     { "urn:another-key", "another-test-value" }
                                 };

            IKeyValueConfiguration appSettingsKeyValueConfiguration = new InMemoryKeyValueConfiguration(collection);

            KVConfigurationManager.Initialize(appSettingsKeyValueConfiguration);

            var goodKeys = new List<string> {
                                   "a-non-existing-key",
                                   "urn:test:key",
                                   "urn:TEST:key",
                                   "urn:test:KEY",
                                   "urn:another-key"
                               };

            var keys = Specials.Special.ToList();
            keys.AddRange(goodKeys.Select(goodKey => new KeyValuePair<string, string>(goodKey, goodKey)));

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