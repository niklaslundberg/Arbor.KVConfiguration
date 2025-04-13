using System.Collections.Generic;
using System.Collections.Immutable;

namespace Arbor.KVConfiguration.JsonConfiguration;

internal static class ImmutabilityExtensions
{
    internal static ImmutableArray<T> SafeToImmutableArray<T>(this IEnumerable<T>? enumerable)
    {
        if (enumerable is ImmutableArray<T> array)
        {
            return array;
        }

        return enumerable?.ToImmutableArray() ?? ImmutableArray<T>.Empty;
    }
}