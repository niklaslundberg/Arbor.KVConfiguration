using System;
using System.Collections.Immutable;
using System.Collections.Specialized;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Schema;
using Arbor.KVConfiguration.Schema.Validators;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit
{
    [Subject(typeof(ConfigurationValidator))]
    public class when_validating_a_valid_required_value_from_source
    {
        static ConfigurationValidator configuration_validator;

        static IKeyValueConfiguration configuration;

        static KeyValueConfigurationValidationSummary summary;

        static ImmutableArray<KeyMetadata> metdata;

        Establish context = () =>
        {
            configuration_validator = new ConfigurationValidator();
            configuration =
                new Core.InMemoryKeyValueConfiguration(new NameValueCollection()
                {
                    {"urn:a:dummy:key:field:constant:urn-value", "a-required-value-fulfilled"}
                });

            ImmutableArray<KeyValueConfigurationItem> configurationItems =
                new ReflectionConfiguratonReader().ReadConfiguration(typeof(when_validating_a_valid_required_value_from_source).Assembly);

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
