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
        static AttributeMetadataSource attribute_metadata_source;
        static ImmutableArray<ConfigurationMetadata> metadata_from_assembly_types;

        Establish context = () => { attribute_metadata_source = new AttributeMetadataSource(); };

        Because of =
            () =>
            {
                metadata_from_assembly_types =
                    attribute_metadata_source.GetMetadataFromAssemblyTypes(
                        typeof(when_getting_configuration_keys_from_code).Assembly);
            };

        It should_ = () =>
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
