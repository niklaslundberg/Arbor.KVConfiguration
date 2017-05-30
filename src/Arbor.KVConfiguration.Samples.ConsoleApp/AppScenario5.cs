using System;
using System.Collections.Immutable;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Schema;
using Newtonsoft.Json;

namespace Arbor.KVConfiguration.Samples.ConsoleApp
{
    public class AppScenario5
    {
        public void Execute()
        {
            KeyValueConfigurationManager.Initialize(
                new ReflectionKeyValueConfiguration(typeof(SampleConfigurationConstants).Assembly));

            var attributeMetadataSource = new AttributeMetadataSource();

            ImmutableArray<ConfigurationMetadata> metadataFromAssemblyTypes =
                attributeMetadataSource.GetMetadataFromAssemblyTypes(typeof(SampleConfigurationConstants).Assembly);

            Console.WriteLine(JsonConvert.SerializeObject(metadataFromAssemblyTypes, Formatting.Indented));

            Console.WriteLine("Contains {0} keys", KeyValueConfigurationManager.AppSettings.AllKeys.Length);

            foreach (StringPair stringPair in KeyValueConfigurationManager.AppSettings.AllValues)
            {
                Console.WriteLine(stringPair);
            }
        }
    }
}
