using System.Collections.Generic;
using System.Collections.Immutable;

namespace Arbor.KVConfiguration.Core.Extensions
{
    public static class ImmutabilityExtensions
    {
        public static ImmutableArray<T> SafeToImmutableArray<T>(this IEnumerable<T> enumerable)
        {
            return enumerable?.ToImmutableArray() ?? ImmutableArray<T>.Empty;
        }
    }
}
