using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Core.Extensions.StringExtensions;

namespace Arbor.KVConfiguration.Samples.ConsoleApp
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

            KeyValueConfigurationManager.Add(appSettingsKeyValueConfiguration).Build();

            var goodKeys = new List<string>
            {
                "a-non-existing-key",
                "urn:test:key",
                "urn:TEST:key",
                "urn:test:KEY",
                "urn:another-key"
            };

            List<KeyValuePair<string, string>> keys = Specials.Special.ToList();
            keys.AddRange(goodKeys.Select(goodKey => new KeyValuePair<string, string>(goodKey, goodKey)));

            var builder = new StringBuilder();

            foreach (KeyValuePair<string, string> pair in keys)
            {
                builder.AppendLine($"Key: {pair.Key}");

                string value = appSettingsKeyValueConfiguration.ValueOrDefault(pair.Value);

                string displayValue = Specials.GetDisplayValue(value);

                builder.AppendLine($"\t Instance: {displayValue}");

                string staticValue = StaticKeyValueConfigurationManager.AppSettings[pair.Value];

                string staticDisplayValue = Specials.GetDisplayValue(staticValue);

                builder.AppendLine($"\t Static: {staticDisplayValue}");
            }

            Console.WriteLine(builder.ToString());
        }
    }
}
