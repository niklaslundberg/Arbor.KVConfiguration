using System;

namespace Arbor.KVConfiguration.Urns
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class UrnAttribute : Attribute
    {
        public UrnAttribute(string urn)
        {
            if (string.IsNullOrWhiteSpace(urn))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(urn));
            }

            Urn = new Urn(urn);
        }

        public Urn Urn { get; }
    }
}
