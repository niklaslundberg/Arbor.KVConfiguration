using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Schema
{
    internal static class ImmutabilityExtensions
    {
        public static ImmutableArray<T> ThrowIfDefault<T>(this ImmutableArray<T> array)
        {
            if (array.IsDefault)
            {
                throw new InvalidOperationException("The array must be initialized");
            }

            return array;
        }

        internal static ImmutableArray<T> SafeToImmutableArray<T>([CanBeNull] this IEnumerable<T> enumerable)
        {
            if (enumerable is ImmutableArray<T> array && !array.IsDefault)
            {
                return array;
            }

            return enumerable?.ToImmutableArray() ?? ImmutableArray<T>.Empty;
        }

        internal static ImmutableArray<T> ValueToImmutableArray<T>(this T item) => new[] { item }.ToImmutableArray();
    }
}