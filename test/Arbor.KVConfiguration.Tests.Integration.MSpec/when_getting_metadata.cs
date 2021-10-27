using System;
using System.Collections.Immutable;
using System.IO;
using Arbor.KVConfiguration.Core.Metadata;
using Arbor.KVConfiguration.JsonConfiguration;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Integration.MSpec
{
    [Subject(typeof(KeyValueConfigurationItemExtensions))]
    public class when_getting_metadata
    {
        static string app_settings_full_path = null!;

        static ImmutableArray<KeyValueConfigurationItem> key_value_configuration_items;

        static JsonFileReader json_file_reader = null!;

        static ImmutableArray<KeyMetadata> metadata;

        Establish context = () =>
        {
            app_settings_full_path = Path.Combine(
                Aesculus.NCrunch.VcsTestPathHelper.TryFindVcsRootPath()!,
                "test",
                "Arbor.KVConfiguration.Tests.Integration",
                "appsettings.json");

            json_file_reader = new JsonFileReader(app_settings_full_path);
            key_value_configuration_items = json_file_reader.ReadConfiguration();
        };

        Because of = () => { metadata = key_value_configuration_items.GetMetadata(); };

        It should_get_metadata_for_every_first_unique_key = () =>
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