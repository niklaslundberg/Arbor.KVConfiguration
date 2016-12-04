﻿using System;
using System.Collections.Generic;
using System.IO;
using Arbor.KVConfiguration.JsonConfiguration;
using Arbor.KVConfiguration.Schema;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Integration
{
    [Subject(typeof(KeyValueConfigurationItemExtensions))]
    public class when_getting_metadata
    {
        static string appsettings_full_path;

        static IReadOnlyCollection<KeyValueConfigurationItem> key_value_configuration_items;

        static JsonFileReader json_file_reader;

        static IReadOnlyCollection<KeyMetadata> metadata;

        Establish context = () =>
        {
            appsettings_full_path = Path.Combine(
                VcsTestPathHelper.FindVcsRootPath(),
                "src",
                "Arbor.KVConfiguration.Tests.Integration",
                "appsettings.json");

            json_file_reader = new JsonFileReader(appsettings_full_path);
            key_value_configuration_items = json_file_reader.ReadConfiguration();
        };

        Because of = () => { metadata = key_value_configuration_items.GetMetadata(); };

        It should_get_metadata_for_every_first_unique_key = () =>
        {
            foreach (KeyMetadata keyValueConfigurationItem in metadata)
            {
                Console.WriteLine(keyValueConfigurationItem.Key);
                Console.WriteLine("  " + keyValueConfigurationItem.Metadata);
            }

            key_value_configuration_items.Count.ShouldEqual(3);
        };
    }
}
