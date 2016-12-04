using System;
using System.Collections.Generic;

using Arbor.KVConfiguration.JsonConfiguration;
using Arbor.KVConfiguration.Schema;

using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit
{
    [Subject(typeof(ConfigurationValidator))]
    public class when_validating_a_missing_required_value
    {
        static ConfigurationValidator configuration_validator;

        static JsonKeyValueConfiguration configuration;

        static KeyValueConfigurationValidationSummary summary;

        static IReadOnlyCollection<KeyMetadata> metdata;

        Establish context = () =>
            {
                configuration_validator = new ConfigurationValidator();

                var configurationItems = new List<KeyValueConfigurationItem>
                                             {
                                                 new KeyValueConfigurationItem(
                                                     key: "abc",
                                                     value: string.Empty,
                                                     metadata:
                                                     new Metadata(
                                                     key: "abc",
                                                     valueType: "string",
                                                     isRequired: true))
                                             };

                metdata = configurationItems.GetMetadata();

                configuration = new JsonKeyValueConfiguration(configurationItems);
            };

        Because of = () => { summary = configuration.AllWithMultipleValues.Validate(configuration_validator, metdata); };

        It should_have_validation_errors = () =>
            {
                Console.WriteLine(summary.Print());
                summary.IsValid.ShouldBeFalse();
            };
    }
}
