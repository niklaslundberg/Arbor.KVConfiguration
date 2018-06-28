using System.Collections.Generic;
using Arbor.KVConfiguration.Urns;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Tests.Integration
{
    [Urn("urn:simple")]
    [UsedImplicitly]
    public class SimpleCtorType
    {
        private sealed class NameAgeEqualityComparer : IEqualityComparer<SimpleCtorType>
        {
            public bool Equals(SimpleCtorType x, SimpleCtorType y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null))
                {
                    return false;
                }

                if (ReferenceEquals(y, null))
                {
                    return false;
                }

                if (x.GetType() != y.GetType())
                {
                    return false;
                }

                return string.Equals(x.Name, y.Name) && x.Age == y.Age;
            }

            public int GetHashCode(SimpleCtorType obj)
            {
                unchecked
                {
                    return ((obj.Name != null ? obj.Name.GetHashCode() : 0) * 397) ^ obj.Age;
                }
            }
        }

        public static IEqualityComparer<SimpleCtorType> NameAgeComparer { get; } = new NameAgeEqualityComparer();

        public string Name { get; }
        public int Age { get; }

        public SimpleCtorType(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }
}
