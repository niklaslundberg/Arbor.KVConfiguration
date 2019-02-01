using System;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Urns
{
    public sealed class NamedInstance<T> : INamedInstance<T>
    {
        public NamedInstance([NotNull] T value, [NotNull] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            }

            Value = value;
            Name = name;
        }

        public T Value { get; }

        public string Name { get; }

        public override string ToString()
        {
            return $"{Name} [{Value}]";
        }
    }
}
