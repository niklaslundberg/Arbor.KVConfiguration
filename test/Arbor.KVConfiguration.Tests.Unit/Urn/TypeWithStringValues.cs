using Arbor.KVConfiguration.Urns;

using Microsoft.Extensions.Primitives;

namespace Arbor.KVConfiguration.Tests.Unit.Urn;

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