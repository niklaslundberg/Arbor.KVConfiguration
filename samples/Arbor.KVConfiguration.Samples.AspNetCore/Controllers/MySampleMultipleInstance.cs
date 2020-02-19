using Arbor.KVConfiguration.Urns;

namespace Arbor.KVConfiguration.Samples.AspNetCore.Controllers
{
    [Urn(Urn)]
    public class MySampleMultipleInstance
    {
        public const string Urn = "urn:vnd:my-sample-multiple";

        public MySampleMultipleInstance(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public string Name { get; }
        public int Age { get; }
    }
}