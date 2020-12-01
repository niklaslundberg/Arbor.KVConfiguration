using System;
using System.Collections.Immutable;
using Arbor.KVConfiguration.Core.Extensions.ReflectionExtensions;
using Arbor.KVConfiguration.Core.Metadata;
using Machine.Specifications;
using Newtonsoft.Json;

namespace Arbor.KVConfiguration.Tests.Unit.Schema
{
    [Subject(typeof(ReflectionAttributeMetadataExtensions))]
    public class when_getting_configuration_keys_from_code
    {
        private static ImmutableArray<ConfigurationMetadata> metadata_from_assembly_types;

        private Establish context = () => { };

        private Because of =
            () =>
            {
                metadata_from_assembly_types =
                    typeof(when_getting_configuration_keys_from_code).Assembly.GetMetadataFromAssemblyTypes();
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