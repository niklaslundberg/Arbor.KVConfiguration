using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Urns
{
    public sealed class NamedInstance<T> : INamedInstance<T>, IEquatable<NamedInstance<T>>
    {
        public bool Equals(NamedInstance<T> other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return EqualityComparer<T>.Default.Equals(Value, other.Value) && string.Equals(Name, other.Name, StringComparison.InvariantCulture);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is NamedInstance<T> other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<T>.Default.GetHashCode(Value) * 397) ^ Name.GetHashCode();
            }
        }

        public static bool operator ==(NamedInstance<T> left, NamedInstance<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NamedInstance<T> left, NamedInstance<T> right)
        {
            return !Equals(left, right);
        }

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
            return $"{GetType().GenericTypeArguments[0].FullName}:'{Name}' [{Value}]";
        }
    }
}
