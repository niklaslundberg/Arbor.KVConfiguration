using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Arbor.KVConfiguration.Core.Metadata;
using Arbor.KVConfiguration.JsonConfiguration;
using Arbor.KVConfiguration.Schema;
using Arbor.KVConfiguration.Schema.Validators;
using Arbor.KVConfiguration.Urns;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit
{
    [Subject(typeof(ConfigurationValidator))]
    public class when_validating_a_non_urn_as_urn
    {
        private static ConfigurationValidator configuration_validator;

        private static JsonKeyValueConfiguration configuration;

        private static KeyValueConfigurationValidationSummary summary;

        private static ImmutableArray<KeyMetadata> metadata;

        private Establish context = () =>
        {
            configuration_validator =
                new ConfigurationValidator(new IValueValidator[] {new UrnValidator()}.ToImmutableArray());

            var configurationItems = new List<KeyValueConfigurationItem>
            {
                new KeyValueConfigurationItem(
                    "abc",
                    "http://example.org",
                    new ConfigurationMetadata(
                        "abc",
                        "urn",
                        isRequired: false))
            };

            metadata = configurationItems.GetMetadata();

            configuration = new JsonKeyValueConfiguration(configurationItems);
        };

        private Because of = () => summary = configuration.AllWithMultipleValues.Validate(configuration_validator, metadata);

        private It should_have_validation_errors = () =>
        {
            Console.WriteLine(configuration.AllWithMultipleValues.Print());

            Console.WriteLine(summary.Print());

            summary.IsValid.ShouldBeFalse();
        };
    }
}