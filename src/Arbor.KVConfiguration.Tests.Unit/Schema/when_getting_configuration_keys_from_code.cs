using System;
using System.Collections.Immutable;
using Arbor.KVConfiguration.Schema;
using Machine.Specifications;
using Newtonsoft.Json;

namespace Arbor.KVConfiguration.Tests.Unit.Schema
{
    [Subject(typeof(AttributeMetadataSource))]
    public class when_getting_configuration_keys_from_code
    {
        private static AttributeMetadataSource attribute_metadata_source;
        private static ImmutableArray<ConfigurationMetadata> metadata_from_assembly_types;

        private Establish context = () => { attribute_metadata_source = new AttributeMetadataSource(); };

        private Because of =
            () =>
            {
                metadata_from_assembly_types =
                    attribute_metadata_source.GetMetadataFromAssemblyTypes(
                        typeof(when_getting_configuration_keys_from_code).Assembly);
            };

        private It should_ = () =>
        {
            foreach (ConfigurationMetadata metadataFromAssemblyType in metadata_from_assembly_types)
            {
                Console.WriteLine(metadataFromAssemblyType);
            }

            Console.WriteLine(JsonConvert.SerializeObject(metadata_from_assembly_types, Formatting.Indented));

            metadata_from_assembly_types.ShouldNotBeEmpty();
        };
    }
}
