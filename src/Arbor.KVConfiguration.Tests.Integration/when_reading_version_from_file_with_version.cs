﻿using System;
using System.IO;

using Arbor.KVConfiguration.JsonConfiguration;
using Arbor.KVConfiguration.Schema.Json;

using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Integration
{
    [Subject(typeof(JsonFileReader))]
    public class when_reading_version_from_file_with_version
    {
        private static string appsettings_full_path;

        private static ConfigurationItems configuration_items;

        private static JsonFileReader reader;

        Establish context = () =>
            {
                appsettings_full_path = Path.Combine(
                    VcsTestPathHelper.FindVcsRootPath(),
                    "src",
                    "Arbor.KVConfiguration.Tests.Integration",
                    "appsettings.json");
                reader = new JsonFileReader(appsettings_full_path);
            };

        Because of = () => { configuration_items = reader.GetConfigurationItems(); };

        It should_have_implicit_version = () => { configuration_items.Version.ShouldEqual("99.0"); };

        It should_have_three_values = () =>
            {
                foreach (var item in configuration_items.Keys)
                {
                    Console.WriteLine(item.Key);
                    Console.WriteLine(": ");
                    Console.WriteLine(item.Value);
                }

                configuration_items.Keys.Count.ShouldEqual(3);
            };
    }
}