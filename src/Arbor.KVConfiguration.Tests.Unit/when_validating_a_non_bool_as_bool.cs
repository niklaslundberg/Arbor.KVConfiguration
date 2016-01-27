using System;
using System.Collections.Generic;
using Arbor.KVConfiguration.JsonConfiguration;
using Arbor.KVConfiguration.Schema;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit
{
    [Subject(typeof(ConfigurationValidator))]
    public class when_validating_a_non_bool_as_bool
    {
        private static ConfigurationValidator configuration_validator;

        private static JsonKeyValueConfiguration configuration;

        private static KeyValueConfigurationValidationSummary summary;

        private static IReadOnlyCollection<KeyMetadata> metdata;

        Establish context = () => {
                                      configuration_validator = new ConfigurationValidator();

                                      var configurationItems = new List<KeyValueConfigurationItem>
                                      {
                                          new KeyValueConfigurationItem(
                                              key: "abc",
                                              value: "xyz",
                                              metadata:
                                                  new Metadata(
                                                      key: "abc",
                                                      valueType: "bool",
                                                      isRequired: false))
                                      };

                                      metdata = configurationItems.GetMetadata();

                                      configuration = new JsonKeyValueConfiguration(configurationItems);
        };

        Because of = () => { summary = configuration.AllWithMultipleValues.Validate(configuration_validator, metdata); };

        It should_have_validation_errors = () => {
                                                     Console.WriteLine(configuration.AllWithMultipleValues.Print());

                                                     Console.WriteLine(summary.Print());

                                                     summary.IsValid.ShouldBeFalse();
        };
    }
}