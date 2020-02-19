using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Arbor.KVConfiguration.Core.Metadata;
using Arbor.KVConfiguration.JsonConfiguration;
using Arbor.KVConfiguration.Schema;
using Arbor.KVConfiguration.Schema.Validators;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit
{
    [Subject(typeof(ConfigurationValidator))]
    public class when_validating_a_non_uri_as_uri
    {
        private static ConfigurationValidator configuration_validator;

        private static JsonKeyValueConfiguration configuration;

        private static KeyValueConfigurationValidationSummary summary;

        private static ImmutableArray<KeyMetadata> metdata;

        private Establish context = () =>
        {
            configuration_validator = new ConfigurationValidator();

            var configurationItems = new List<KeyValueConfigurationItem>
            {
                new KeyValueConfigurationItem(
                    "abc",
                    "[]!",
                    new ConfigurationMetadata(
                        "abc",
                        "uri",
                        isRequired: false))
            };

            metdata = configurationItems.GetMetadata();

            configuration = new JsonKeyValueConfiguration(configurationItems);
        };

        private Because of = () =>
        {
            summary = configuration.AllWithMultipleValues.Validate(configuration_validator, metdata);
        };

        private It should_have_validation_errors = () =>
        {
            Console.WriteLine(configuration.AllWithMultipleValues.Print());

            Console.WriteLine(summary.Print());

            summary.IsValid.ShouldBeFalse();
        };
    }
}