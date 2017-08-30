using System;
using System.Text;

namespace Arbor.KVConfiguration.Core
{
    public struct StringPair : IEquatable<StringPair>
    {
        public StringPair(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; }

        public string Value { get; }

        public static bool operator ==(StringPair left, StringPair right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(StringPair left, StringPair right)
        {
            return !left.Equals(right);
        }

        public bool Equals(StringPair other)
        {
            return string.Equals(Key, other.Key, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is StringPair && Equals((StringPair)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Key?.GetHashCode() ?? 0) * 397) ^ (Value?.GetHashCode() ?? 0);
            }
        }

        public override string ToString()
        {
            var toStringBuilder = new StringBuilder(Key?.Length + Value?.Length + 4 ?? 10);
            toStringBuilder.Append('[');
            if (Key != null)
            {
                toStringBuilder.Append(Key);
            }

            toStringBuilder.Append(", \"");
            if (Value != null)
            {
                toStringBuilder.Append(Value);
            }

            toStringBuilder.Append("\"]");
            return toStringBuilder.ToString();
        }
    }
}
