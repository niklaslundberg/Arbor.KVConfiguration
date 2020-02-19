using System;
using System.IO;
using Arbor.KVConfiguration.JsonConfiguration;

namespace Arbor.KVConfiguration.Tests.Integration
{
    [Subject(typeof(JsonFileReader))]
    public class when_reading_version_from_file_with_version
    {
        private static string appsettings_full_path;

        private static ConfigurationItems configuration_items;

        private static JsonFileReader reader;

        private Establish context = () =>
        {
            appsettings_full_path = Path.Combine(
                VcsTestPathHelper.FindVcsRootPath(),
                "test",
                "Arbor.KVConfiguration.Tests.Integration",
                "appsettings.json");

            reader = new JsonFileReader(appsettings_full_path);
        };

        private Because of = () => { configuration_items = reader.GetConfigurationItems(); };

        private It should_have_implicit_version = () => { configuration_items.Version.ShouldEqual("99.0"); };

        private It should_have_three_values = () =>
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