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
    public class when_validating_timespan_as_timespan
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
                    "0:30:5",
                    new ConfigurationMetadata(
                        "abc",
                        "timespan",
                        isRequired: false))
            };

            metdata = configurationItems.GetMetadata();

            configuration = new JsonKeyValueConfiguration(configurationItems);
        };

        private Because of = () => summary = configuration.AllWithMultipleValues.Validate(configuration_validator, metdata);

        private It should_have_no_validation_errors = () =>
        {
            Console.WriteLine(configuration.AllWithMultipleValues.Print());

            Console.WriteLine(summary.Print());

            summary.IsValid.ShouldBeTrue();
        };
    }
}