using Arbor.KVConfiguration.Urns;

namespace Arbor.KVConfiguration.Samples.AspNetCore
{
    [Urn(ConfigurationKeys.DummyKey)]
    public class MySampleConfiguration
    {
        public MySampleConfiguration(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public string Name { get; }

        public int Age { get; }
    }
}
