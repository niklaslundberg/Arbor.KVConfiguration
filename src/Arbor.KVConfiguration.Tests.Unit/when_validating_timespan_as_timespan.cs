using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Arbor.KVConfiguration.JsonConfiguration;
using Arbor.KVConfiguration.Schema;
using Arbor.KVConfiguration.Schema.Validators;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit
{
    [Subject(typeof(ConfigurationValidator))]
    public class when_validating_timespan_as_timespan
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
                                              value: "0:30:5",
                                              configurationMetadata:
                                                  new ConfigurationMetadata(
                                                      key: "abc",
                                                      valueType: "timespan",
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
