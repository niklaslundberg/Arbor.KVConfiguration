using System;

namespace Arbor.KVConfiguration.Urns;

public static class NamedInstanceExtensions
{
    public static string EnclosingTypeName<T>(this INamedInstance<T> instance)
    {
        instance.ThrowIfNull(nameof(instance));

        return instance.GetType().GenericTypeArguments[0].FullName ?? throw new InvalidOperationException(
            $"Could not get enclosing full name for type {typeof(T).FullName}");
    }
}