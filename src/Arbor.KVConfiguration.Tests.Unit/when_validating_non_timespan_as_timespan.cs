using System;
using System.Collections.Generic;
using Arbor.KVConfiguration.JsonConfiguration;
using Arbor.KVConfiguration.Schema;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit
{
    [Subject(typeof (ConfigurationValidator))]
    public class when_validating_non_timespan_as_timespan
    {
        private static ConfigurationValidator configuration_validator;

        private static JsonKeyValueConfiguration configuration;

        private static KeyValueConfigurationValidationSummary summary;

        private static IReadOnlyCollection<KeyMetadata> metdata;

        private Establish context = () =>
        {
            configuration_validator = new ConfigurationValidator();

            var configurationItems = new List<KeyValueConfigurationItem>
            {
                new KeyValueConfigurationItem("abc", "xyz", new Metadata("abc", "timespan",
                    isRequired: false))
            };

            metdata = configurationItems.GetMetadata();

            configuration = new JsonKeyValueConfiguration(configurationItems);
        };

        private Because of =
            () => { summary = configuration.AllWithMultipleValues.Validate(configuration_validator, metdata); };

        private It should_have_validation_errors = () =>
        {
            Console.WriteLine(configuration.AllWithMultipleValues.Print());

            Console.WriteLine(summary.Print());

            summary.IsValid.ShouldBeFalse();
        };
    }
}
