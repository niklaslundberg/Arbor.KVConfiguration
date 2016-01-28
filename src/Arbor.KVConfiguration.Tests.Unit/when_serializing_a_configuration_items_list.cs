using System;
using System.Collections.Generic;
using Arbor.KVConfiguration.Schema.Json;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit
{
    public class when_serializing_a_configuration_items_list
    {
        private static JsonConfigurationSerializer _serializer;

        private static ConfigurationItems configurationItems;

        private static string _serialized;

        private Establish context = () =>
        {
            _serializer = new JsonConfigurationSerializer();
            IReadOnlyCollection<KeyValue> keys = new List<KeyValue>(10)
            {
                new KeyValue("abc", "123", null),
                new KeyValue("def", "234", null),
                new KeyValue("ghi", "345", null),
                new KeyValue("ghi", "456", null)
            };
            configurationItems = new ConfigurationItems("1.0", keys);
        };

        private Because of = () => { _serialized = _serializer.Serialize(configurationItems); };

        private It should_have_serialized_with_version_first_then_properties_in_order =
            () => { Console.WriteLine(_serialized); };
    }
}
