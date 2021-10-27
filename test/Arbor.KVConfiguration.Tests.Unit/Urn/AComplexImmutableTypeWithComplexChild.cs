using System;
using System.Diagnostics.CodeAnalysis;
using Arbor.KVConfiguration.Urns;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [PublicAPI]
    [Urn("urn:test:a:complex:immutable:type-with-complex-child")]
    public class AComplexImmutableTypeWithComplexChild
    {
        public AComplexImmutableTypeWithComplexChild(string id, string name, ComplexChild child, Uri? uri = null)
        {
            Id = id;
            Name = name;
            Child = child;
            Uri = uri;
        }

        public string Id { get; }

        public string Name { get; }

        public ComplexChild Child { get; }

        public Uri? Uri { get; }
    }
}