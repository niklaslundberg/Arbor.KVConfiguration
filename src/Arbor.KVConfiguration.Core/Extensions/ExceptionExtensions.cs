using System;
using System.Runtime.CompilerServices;

namespace Arbor.KVConfiguration.Core.Extensions;

internal static class ExceptionExtensions
{
    public static void ThrowIfNull<T>(this T? value, [CallerArgumentExpression(nameof(value))] string? name = null)
        where T : class
    {
#if NET6_0_OR_GREATER
#else
        throw new ArgumentException("Value cannot be null.", name);
#endif
    }
}