using System;
using Arbor.KVConfiguration.Core;
using Arbor.Primitives;

namespace Arbor.KVConfiguration.Urns
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class UrnAttribute : Attribute
    {
        public UrnAttribute(string urn)
        {
            if (string.IsNullOrWhiteSpace(urn))
            {
                throw new ArgumentException(KeyValueResources.ArgumentIsNullOrWhitespace, nameof(urn));
            }

            Urn = new Urn(urn);
        }

        public Urn Urn { get; }
    }
}
