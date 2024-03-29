﻿using System.Collections.Specialized;
using Arbor.KVConfiguration.Core.Extensions.StringExtensions;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.InMemoryKeyValueConfiguration
{
    [Subject(typeof(Core.InMemoryKeyValueConfiguration))]
    public class when_getting_an_existing_value_with_specific_default_value
    {
        private static Core.InMemoryKeyValueConfiguration configuration;

        private static string? value;

        private Establish context = () => configuration = new Core.InMemoryKeyValueConfiguration(new NameValueCollection { { "a", "b" } });

        private Because of = () => value = configuration.ValueOrDefault("a", "c");

        private It return_existing_value = () => value.ShouldEqual("b");
    }
}