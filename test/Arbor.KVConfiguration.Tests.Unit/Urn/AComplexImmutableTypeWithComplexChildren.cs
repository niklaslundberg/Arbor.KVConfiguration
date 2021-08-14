using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Arbor.KVConfiguration.Urns;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Urn("urn:test:a:complex:immutable:type-with-complex-children")]
    public class AComplexImmutableTypeWithComplexChildren
    {
        public AComplexImmutableTypeWithComplexChildren(string id,
            string name,
            IEnumerable<ComplexChild> children,
            Uri? uri = null)
        {
            Id = id;
            Name = name;
            Children = children?.ToImmutableArray() ?? ImmutableArray<ComplexChild>.Empty;
            Uri = uri;
        }

        public string Id { get; }

        public string Name { get; }

        public ImmutableArray<ComplexChild> Children { get; }

        public Uri? Uri { get; }
    }
}