﻿using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Core.Extensions.StringExtensions;

namespace Arbor.KVConfiguration.Tests.Integration.AppSettingsKeyValueConfiguration
{
    [Subject(typeof(SystemConfiguration.AppSettingsKeyValueConfiguration))]
    public class when_getting_a_non_existing_value_with_implicit_default_value
    {
        private static IKeyValueConfiguration configuration;

        private static string value;

        private Establish context = () =>
        {
            configuration = new SystemConfiguration.AppSettingsKeyValueConfiguration();
        };

        private Because of = () => { value = configuration.ValueOrDefault("d"); };

        private It return_existing_value = () => { value.ShouldEqual(""); };
    }
}