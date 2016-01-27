using System;
using System.Collections.Generic;
using System.Linq;

using Arbor.KVConfiguration.Schema;
using Arbor.KVConfiguration.Schema.Json;

using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit
{
    [Subject(typeof(ConfigurationSerializer))]
    public class when_serializing_and_deserializing_two_keys
    {
        private static ConfigurationItems configurationItems;

        private static string json;

        private static ConfigurationItems restored_configuration;

        private static ConfigurationSerializer serializer;

        Establish context = () =>
            {
                configurationItems = new ConfigurationItems(
                    "1.0",
                    new List<KeyValue>
                        {
                            new KeyValue(
                                "a",
                                "1",
                                new Metadata(
                                "a",
                                "string",
                                "A",
                                "A description",
                                "ATest",
                                "ATestFullName",
                                typeof(when_serializing_and_deserializing_two_keys),
                                0,
                                "1.txt",
                                true,
                                "A default",
                                "A note",
                                false,
                                new[] { "A example" },
                                new[] { "A tag" })),
                            new KeyValue("b", "2", null)
                        });

                serializer = new ConfigurationSerializer();
                json = serializer.Serialize(configurationItems);
            };

        Because of = () => { restored_configuration = serializer.Deserialize(json); };

        It should_have_first_correct_key = () => { restored_configuration.Keys.First().Key.ShouldEqual("a"); };

        It should_have_first_correct_value = () => { restored_configuration.Keys.First().Value.ShouldEqual("1"); };

        It should_have_last_correct_key = () => { restored_configuration.Keys.Last().Key.ShouldEqual("b"); };

        It should_have_last_correct_value = () => { restored_configuration.Keys.Last().Value.ShouldEqual("2"); };

        It should_have_metadata_for_the_first_item =
            () => { restored_configuration.Keys.First().Metadata.ShouldNotBeNull(); };

        It should_have_two_keys = () =>
            {
                Console.WriteLine(json);
                restored_configuration.Keys.Count.ShouldEqual(2);
            };
    }
}
