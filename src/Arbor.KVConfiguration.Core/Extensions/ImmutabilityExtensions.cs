using System.Collections.Generic;
using System.Collections.Immutable;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core.Extensions
{
    internal static class ImmutabilityExtensions
    {
        [NotNull]
        internal static ImmutableArray<T> SafeToImmutableArray<T>([CanBeNull] this IEnumerable<T> enumerable)
        {
            if (enumerable is ImmutableArray<T> array)
            {
                return array;
            }

            return enumerable?.ToImmutableArray() ?? ImmutableArray<T>.Empty;
        }
    }
}