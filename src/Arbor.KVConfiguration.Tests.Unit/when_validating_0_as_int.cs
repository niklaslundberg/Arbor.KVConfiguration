using System;
using System.Collections.Generic;
using Arbor.KVConfiguration.JsonConfiguration;
using Arbor.KVConfiguration.Schema;
using Machine.Specifications;
using System.Collections.Immutable;
using Arbor.KVConfiguration.Schema.Validators;

namespace Arbor.KVConfiguration.Tests.Unit
{
    [Subject(typeof(ConfigurationValidator))]
    public class when_validating_0_as_int
    {
        static ConfigurationValidator configuration_validator;

        static JsonKeyValueConfiguration configuration;

        static KeyValueConfigurationValidationSummary summary;

        static ImmutableArray<KeyMetadata> metdata;

        Establish context = () => {
                                      configuration_validator = new ConfigurationValidator();

                                      var configurationItems = new List<KeyValueConfigurationItem>
                                      {
                                          new KeyValueConfigurationItem(
                                              key: "abc",
                                              value: "0",
                                              configurationMetadata:
                                                  new ConfigurationMetadata(
                                                      key: "abc",
                                                      valueType: "int",
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
