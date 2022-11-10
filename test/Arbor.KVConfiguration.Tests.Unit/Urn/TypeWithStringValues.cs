using System;
using Arbor.KVConfiguration.Urns;
using JetBrains.Annotations;
using Microsoft.Extensions.Primitives;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [PublicAPI]
    [Urn("urn:a:type:with:string:params")]
    public class TypeWithStringValues
    {
        public TypeWithStringValues(StringValues values, string other = "abc")
        {
            Other = other;
            Values = values.ToArray();
        }

        public string Other { get; }

        public string?[] Values { get; }
    }
}