using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Schema;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit
{
    [Subject(typeof(ConfigurationValidator))]
    public class when_validating_a_valid_required_value_from_source
    {
        private static ConfigurationValidator configuration_validator;

        private static IKeyValueConfiguration configuration;

        private static KeyValueConfigurationValidationSummary summary;

        private static IReadOnlyCollection<KeyMetadata> metdata;

        Establish context = () =>
        {
            configuration_validator = new ConfigurationValidator();
            configuration =
                new Core.InMemoryKeyValueConfiguration(new NameValueCollection()
                {
                    {"urn:a:dummy:key:field:constant:urn-value", "a-required-value-fulfilled"}
                });

            IReadOnlyCollection<KeyValueConfigurationItem> configurationItems =
                new SourceReader().ReadConfiguration(typeof(when_validating_a_valid_required_value_from_source).Assembly);

            metdata = configurationItems.GetMetadata();

        };

        Because of = () => { summary = configuration.AllWithMultipleValues.Validate(configuration_validator, metdata); };

        It should_have_no_validation_errors = () =>
        {
            Console.WriteLine(summary.Print());

            summary.IsValid.ShouldBeTrue();
        };
    }
}
