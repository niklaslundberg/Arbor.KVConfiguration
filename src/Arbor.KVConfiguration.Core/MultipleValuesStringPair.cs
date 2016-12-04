using System;
using System.Collections.Immutable;
using System.Linq;

namespace Arbor.KVConfiguration.Core
{
    public struct MultipleValuesStringPair : IEquatable<MultipleValuesStringPair>
    {
        public string Key { get; }

        public ImmutableArray<string> Values { get; }

        public MultipleValuesStringPair(string key, ImmutableArray<string> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            Key = key;
            Values = values;
            HasNonEmptyValue = values.Any(value => !string.IsNullOrWhiteSpace(value));
        }

        public bool Equals(MultipleValuesStringPair other)
        {
            return string.Equals(Key, other.Key) && Values.SequenceEqual(other.Values);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is MultipleValuesStringPair && Equals((MultipleValuesStringPair)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Key?.GetHashCode() ?? 0) * 397) ^ Values.GetHashCode();
            }
        }

        public static bool operator ==(MultipleValuesStringPair left, MultipleValuesStringPair right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MultipleValuesStringPair left, MultipleValuesStringPair right)
        {
            return !left.Equals(right);
        }

        public bool HasSingleValue => Values.Length == 1;

        public bool HasNonEmptyValue { get; }
    }
}
