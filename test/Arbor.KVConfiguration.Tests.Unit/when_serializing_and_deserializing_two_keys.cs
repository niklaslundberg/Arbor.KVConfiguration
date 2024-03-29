﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Arbor.KVConfiguration.Core.Metadata;
using Arbor.KVConfiguration.Schema.Json;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit
{
    [Subject(typeof(JsonConfigurationSerializer))]
    public class when_serializing_and_deserializing_two_keys
    {
        private static ConfigurationItems configuration_items;

        private static string json;

        private static ConfigurationItems restored_configuration;

        private Establish context = () =>
        {
            var keyValues = new List<KeyValue>(2)
            {
                new KeyValue(
                    "a",
                    "1",
                    new ConfigurationMetadata(
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
                        new[] {"A example"},
                        new[] {"A tag"})),
                new KeyValue("b", "2", null)
            };

            configuration_items = new ConfigurationItems(
                "1.0",
                keyValues.ToImmutableArray());

            json = JsonConfigurationSerializer.Serialize(configuration_items);
        };

        private Because of = () => restored_configuration = JsonConfigurationSerializer.Deserialize(json);

        private It should_have_first_correct_key = () => restored_configuration.Keys[0].Key.ShouldEqual("a");

        private It should_have_first_correct_value = () => restored_configuration.Keys[0].Value.ShouldEqual("1");

        private It should_have_last_correct_key = () => restored_configuration.Keys.Last().Key.ShouldEqual("b");

        private It should_have_last_correct_value = () => restored_configuration.Keys.Last().Value.ShouldEqual("2");

        private It should_have_metadata_for_the_first_item =
            () => restored_configuration.Keys[0].ConfigurationMetadata.ShouldNotBeNull();

        private It should_have_two_keys = () =>
        {
            Console.WriteLine(json);
            restored_configuration.Keys.Length.ShouldEqual(2);
        };
    }
}