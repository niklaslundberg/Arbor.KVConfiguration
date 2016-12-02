using System;
using System.Collections.Generic;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Schema;
using Newtonsoft.Json;

namespace Arbor.KVConfiguration.Samples.ConsoleApp
{
    public class AppScenario5
    {
        public void Execute()
        {
            KVConfigurationManager.Initialize(new SourceKeyValueConfiguration(typeof(SampleConfigurationConstants).Assembly));

            var attributeMetadataSource = new AttributeMetadataSource();

            IReadOnlyCollection<Metadata> metadataFromAssemblyTypes =
                attributeMetadataSource.GetMetadataFromAssemblyTypes(typeof(SampleConfigurationConstants).Assembly);

            Console.WriteLine(JsonConvert.SerializeObject(metadataFromAssemblyTypes, Formatting.Indented));

            Console.WriteLine("Contains {0} keys", KVConfigurationManager.AppSettings.AllKeys.Count);

            foreach (StringPair stringPair in KVConfigurationManager.AppSettings.AllValues)
            {
                Console.WriteLine(stringPair);
            }
        }
    }
}
