﻿using Arbor.KVConfiguration.Core;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Integration.AppSettingsKeyValueConfiguration
{
    [Subject(typeof(SystemConfiguration.AppSettingsKeyValueConfiguration))]
    public class when_getting_an_existing_value_with_specific_default_value
    {
        static IKeyValueConfiguration configuration;

        static string value;

        Establish context = () => { configuration = new SystemConfiguration.AppSettingsKeyValueConfiguration(); };

        Because of = () => { value = configuration.ValueOrDefault("a", "c"); };

        It return_existing_value = () => { value.ShouldEqual("b"); };
    }
}
