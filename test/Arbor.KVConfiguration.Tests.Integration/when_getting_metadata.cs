using System;
using System.Collections.Immutable;
using System.IO;
using Arbor.KVConfiguration.JsonConfiguration;
using Arbor.KVConfiguration.Schema;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Integration
{
    [Subject(typeof(KeyValueConfigurationItemExtensions))]
    public class when_getting_metadata
    {
        private static string appsettings_full_path;

        private static ImmutableArray<KeyValueConfigurationItem> key_value_configuration_items;

        private static JsonFileReader json_file_reader;

        private static ImmutableArray<KeyMetadata> metadata;

        private Establish context = () =>
        {
            appsettings_full_path = Path.Combine(
                VcsTestPathHelper.FindVcsRootPath(),
                "src",
                "Arbor.KVConfiguration.Tests.Integration",
                "appsettings.json");

            json_file_reader = new JsonFileReader(appsettings_full_path);
            key_value_configuration_items = json_file_reader.ReadConfiguration();
        };

        private Because of = () => { metadata = key_value_configuration_items.GetMetadata(); };

        private It should_get_metadata_for_every_first_unique_key = () =>
        {
            foreach (KeyMetadata keyValueConfigurationItem in metadata)
            {
                Console.WriteLine(keyValueConfigurationItem.Key);
                Console.WriteLine("  " + keyValueConfigurationItem.ConfigurationMetadata);
            }

            key_value_configuration_items.Length.ShouldEqual(3);
        };
    }
}
