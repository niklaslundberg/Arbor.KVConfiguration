using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Arbor.KVConfiguration.Core.Extensions;
using Arbor.KVConfiguration.Urns;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Urn("urn:a:complex:immutable:type")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class AComplexImmutableType
    {
        public AComplexImmutableType(string id, string name, IEnumerable<string> children, Uri uri = null)
        {
            Id = id;
            Name = name;
            Children = children.SafeToImmutableArray();
            Uri = uri;
        }

        public string Id { get; }

        public string Name { get; }

        public ImmutableArray<string> Children { get; }

        public Uri Uri { get; }
    }
}
