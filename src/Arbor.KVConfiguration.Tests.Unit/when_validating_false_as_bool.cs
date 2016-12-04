using System;
using System.Collections.Generic;
using Arbor.KVConfiguration.JsonConfiguration;
using Arbor.KVConfiguration.Schema;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit
{
    [Subject(typeof(ConfigurationValidator))]
    public class when_validating_false_as_bool
    {
        static ConfigurationValidator configuration_validator;

        static JsonKeyValueConfiguration configuration;

        static KeyValueConfigurationValidationSummary summary;

        static IReadOnlyCollection<KeyMetadata> metdata;

        Establish context = () => {
                                      configuration_validator = new ConfigurationValidator();

                                      var configurationItems = new List<KeyValueConfigurationItem>
                                      {
                                          new KeyValueConfigurationItem(
                                              key: "abc",
                                              value: "false",
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

        It should_have_no_validation_errors = () => {
                                                        Console.WriteLine(configuration.AllWithMultipleValues.Print());

                                                        Console.WriteLine(summary.Print());

                                                        summary.IsValid.ShouldBeTrue();
        };
    }
}