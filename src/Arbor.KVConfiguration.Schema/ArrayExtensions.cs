using System;

namespace Arbor.KVConfiguration.Schema
{
    internal static class ArrayExtensions<T>
    {
        private static readonly Lazy<T[]> _Array = new Lazy<T[]>(() => new T[] {});

        public static T[] Empty()
        {
            return _Array.Value;
        }
    }
}
