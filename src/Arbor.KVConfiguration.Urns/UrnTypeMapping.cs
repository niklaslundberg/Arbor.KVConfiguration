using System;
using Arbor.Primitives;

namespace Arbor.KVConfiguration.Urns
{
    public class UrnTypeMapping
    {
        public UrnTypeMapping(Type type, Urn urn)
        {
            Type = type;
            Urn = urn;
        }

        public Type Type { get; }
        public Urn Urn { get; }
    }
}