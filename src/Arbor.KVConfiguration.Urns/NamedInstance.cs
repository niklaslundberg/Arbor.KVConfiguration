using System;
using System.Collections.Generic;
using Arbor.KVConfiguration.Core;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Urns
{
    public sealed class NamedInstance<T> : INamedInstance<T>, IEquatable<NamedInstance<T>>
    {
        public NamedInstance([NotNull] T value, [NotNull] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(KeyValueResources.ArgumentIsNullOrWhitespace, nameof(name));
            }

            Value = value;
            Name = name;
        }

        [NotNull]
        public T Value { get; }

        public string Name { get; }

        public override bool Equals(object? obj) => Equals(obj as NamedInstance<T>);

        public bool Equals(NamedInstance<T>? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return EqualityComparer<T>.Default.Equals(Value, other.Value) &&
                   string.Equals(Name, other.Name, StringComparison.InvariantCulture);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<T>.Default.GetHashCode(Value!) * 397) ^ Name.GetHashCode();
            }
        }

        public static bool operator ==(NamedInstance<T> left, NamedInstance<T> right) => Equals(left, right);

        public static bool operator !=(NamedInstance<T> left, NamedInstance<T> right) => !Equals(left, right);

        public override string ToString() => $"{GetType().GenericTypeArguments[0].FullName}:'{Name}' [{Value}]";
    }
}