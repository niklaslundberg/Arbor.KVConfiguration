using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Schema
{
    internal static class ImmutabilityExtensions
    {
        internal static ImmutableArray<T> SafeToImmutableArray<T>([CanBeNull] this IEnumerable<T> enumerable)
        {
            if (enumerable is ImmutableArray<T> array)
            {
                return array;
            }

            return enumerable?.ToImmutableArray() ?? ImmutableArray<T>.Empty;
        }

        internal static ImmutableArray<T> ValueToImmutableArray<T>(this T item)
        {
            return new[] { item }.ToImmutableArray();
        }

        public static ImmutableArray<T> ThrowIfDefault<T>(this ImmutableArray<T> array)
        {
            if (array.IsDefault)
            {
                throw new InvalidOperationException("The array must be initialized");
            }

            return array;
        }
    }
}
