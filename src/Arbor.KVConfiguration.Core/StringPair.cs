using System;
using System.Text;

namespace Arbor.KVConfiguration.Core
{
    public struct StringPair : IEquatable<StringPair>
    {
        public string Key { get; }

        public string Value { get; }

        public StringPair(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public bool Equals(StringPair other)
        {
            return string.Equals(Key, other.Key) && string.Equals(Value, other.Value);
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

        public static bool operator ==(StringPair left, StringPair right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(StringPair left, StringPair right)
        {
            return !left.Equals(right);
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
