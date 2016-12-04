using System;
using System.Collections.Generic;
using Arbor.KVConfiguration.Schema.Json;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit
{
    [Subject(typeof(JsonConfigurationSerializer))]
    public class when_serializing_a_configuration_items_list
    {
        static JsonConfigurationSerializer serializer;

        static ConfigurationItems configuration_items;

        static string serialized;

        Establish context = () =>
        {
            serializer = new JsonConfigurationSerializer();
            IReadOnlyCollection<KeyValue> keys = new List<KeyValue>(10)
            {
                new KeyValue("abc", "123", null),
                new KeyValue("def", "234", null),
                new KeyValue("ghi", "345", null),
                new KeyValue("ghi", "456", null)
            };
            configuration_items = new ConfigurationItems("1.0", keys);
        };

        Because of = () => { serialized = serializer.Serialize(configuration_items); };

        It should_have_serialized_with_version_first_then_properties_in_order =
            () => { Console.WriteLine(serialized); };
    }
}
