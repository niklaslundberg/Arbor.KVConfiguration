using System;
using System.Collections.Generic;
using Arbor.KVConfiguration.Schema;
using Machine.Specifications;
using Newtonsoft.Json;

namespace Arbor.KVConfiguration.Tests.Unit.Schema
{
    [Subject(typeof(AttributeMetadataSource))]
    public class when_getting_configuration_keys_from_code
    {
        private Establish context = () =>
        {

            _attributeMetadataSource = new AttributeMetadataSource();

        };
        private Because of = () =>
        {
            _metadataFromAssemblyTypes = _attributeMetadataSource.GetMetadataFromAssemblyTypes(typeof(when_getting_configuration_keys_from_code).Assembly);
        };

        private It should_ = () =>
        {
            foreach (Metadata metadataFromAssemblyType in _metadataFromAssemblyTypes)
            {
                Console.WriteLine(metadataFromAssemblyType);
            }

            Console.WriteLine(JsonConvert.SerializeObject(_metadataFromAssemblyTypes, Formatting.Indented));

            _metadataFromAssemblyTypes.ShouldNotBeEmpty();
        };

        private static AttributeMetadataSource _attributeMetadataSource;
        private static IReadOnlyCollection<Metadata> _metadataFromAssemblyTypes;
    }
}
