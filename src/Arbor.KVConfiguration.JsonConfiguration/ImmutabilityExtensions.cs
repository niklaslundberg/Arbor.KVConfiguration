﻿using System.Collections.Generic;
using System.Collections.Immutable;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.JsonConfiguration
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
    }
}