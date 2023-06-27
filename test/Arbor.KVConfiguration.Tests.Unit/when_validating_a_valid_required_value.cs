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
    public class when_validating_a_valid_required_value
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
                    "123",
                    new ConfigurationMetadata(
                        "abc",
                        "string"))
            };

            metdata = configurationItems.GetMetadata();

            configuration = new JsonKeyValueConfiguration(configurationItems);
        };

        private Because of = () => summary = configuration.AllWithMultipleValues.Validate(configuration_validator, metdata);

        private It should_have_no_validation_errors = () =>
        {
            Console.WriteLine(summary.Print());

            summary.IsValid.ShouldBeTrue();
        };
    }
}