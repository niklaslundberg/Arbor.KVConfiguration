using System;
using Arbor.KVConfiguration.Urns;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [PublicAPI]
    [Urn("urn:type:with:required:ctor")]
    internal class TypeWithRequiredCtor
    {
        public string Key { get; }

        public TypeWithRequiredCtor([NotNull] string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"Value cannot be null or whitespace from type {nameof(TypeWithRequiredCtor)}", nameof(key));
            }

            Key = key;
        }
    }
}
