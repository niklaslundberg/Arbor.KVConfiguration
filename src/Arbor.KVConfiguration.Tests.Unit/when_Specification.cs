using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Arbor.KVConfiguration.Schema.Json;

using Machine.Specifications;
using Machine.Specifications.Model;

namespace Arbor.KVConfiguration.Tests.Unit
{
    [Subject(typeof(Subject))]
    public class when_Specification
    {
        Establish context = () =>
            { configuration = new Configuration("1.0", new List<KeyValue>()
                                                           {
                                                               new KeyValue("a", "1", null),
                                                               new KeyValue("b", "2", null),
                                                           });

                serializer = new ConfigurationSerializer();
            };

        Because of = () =>
            {
                json = serializer.Serialize(configuration);
            };

        It should_Behaviour = () =>
            {
                Console.WriteLine(json);
            };

        private static Configuration configuration;

        private static ConfigurationSerializer serializer;

        private static string json;
    }
}
