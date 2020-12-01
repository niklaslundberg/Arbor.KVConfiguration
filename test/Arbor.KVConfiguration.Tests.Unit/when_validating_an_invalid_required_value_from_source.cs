using System;
using System.Collections.Immutable;
using System.Collections.Specialized;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Core.Metadata;
using Arbor.KVConfiguration.Schema;
using Arbor.KVConfiguration.Schema.Validators;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit
{
    [Subject(typeof(ConfigurationValidator))]
    public class when_validating_an_invalid_required_value_from_source
    {
        private static ConfigurationValidator configuration_validator;

        private static IKeyValueConfiguration configuration;

        private static KeyValueConfigurationValidationSummary summary;

        private static ImmutableArray<KeyMetadata> metdata;

        private Establish context = () =>
        {
            configuration_validator = new ConfigurationValidator();

            configuration =
                new Core.InMemoryKeyValueConfiguration(new NameValueCollection
                {
                    {"urn:a:dummy:key:field:constant:urn-value", ""}
                });

            ImmutableArray<KeyValueConfigurationItem> configurationItems =
                new ReflectionKeyValueConfiguration(
                    typeof(when_validating_a_valid_required_value_from_source).Assembly).ConfigurationItems;

            metdata = configurationItems.GetMetadata();
        };

        private Because of =
            () => { summary = configuration.AllWithMultipleValues.Validate(configuration_validator, metdata); };

        private It should_have_validation_errors = () =>
        {
            Console.WriteLine(summary.Print());

            summary.IsValid.ShouldBeFalse();
        };
    }
}