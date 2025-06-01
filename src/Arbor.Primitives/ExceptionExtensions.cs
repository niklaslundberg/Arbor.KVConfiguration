using System;

namespace Arbor.Primitives;

internal static class ExceptionExtensions
{
    public static void ThrowIfNull<T>(this T? value, string? name = null)
        where T : class
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(value, name);
#else
            throw new ArgumentException("Value cannot be null.", name);
#endif
    }
}