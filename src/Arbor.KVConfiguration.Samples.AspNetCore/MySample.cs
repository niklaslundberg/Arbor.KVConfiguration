using Arbor.KVConfiguration.Urns;

namespace Arbor.KVConfiguration.Samples.AspNetCore
{
    [Urn(Constants.ConfigurationKeys.DummyKey)]
    public class MySample
    {
        public MySample(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public string Name { get; }
        public int Age { get; }
    }
}
