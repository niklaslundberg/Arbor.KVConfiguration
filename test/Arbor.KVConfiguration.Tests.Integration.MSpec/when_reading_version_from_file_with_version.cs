using System;
using System.IO;
using Arbor.KVConfiguration.JsonConfiguration;
using Arbor.KVConfiguration.Schema.Json;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Integration.MSpec
{
    [Subject(typeof(JsonFileReader))]
    public class when_reading_version_from_file_with_version
    {
        static string appsettings_full_path;

        static ConfigurationItems configuration_items;

        static JsonFileReader reader;

        Establish context = () =>
        {
            appsettings_full_path = Path.Combine(
                VcsTestPathHelper.FindVcsRootPath(),
                "test",
                "Arbor.KVConfiguration.Tests.Integration",
                "appsettings.json");

            reader = new JsonFileReader(appsettings_full_path);
        };

        Because of = () => { configuration_items = reader.GetConfigurationItems(); };

        It should_have_implicit_version = () => { configuration_items.Version.ShouldEqual("99.0"); };

        It should_have_three_values = () =>
        {
            foreach (KeyValue item in configuration_items.Keys)
            {
                Console.WriteLine(item.Key);
                Console.WriteLine(": ");
                Console.WriteLine(item.Value);
            }

            configuration_items.Keys.Length.ShouldEqual(3);
        };
    }
}