using System;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Urns
{
    public static class NamedInstanceExtensions
    {
        public static string EnclosingTypeName<T>([NotNull] this INamedInstance<T> instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return instance.GetType().GenericTypeArguments[0].FullName;
        }
    }
}