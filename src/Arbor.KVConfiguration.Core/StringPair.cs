﻿using System;
using System.Text;

namespace Arbor.KVConfiguration.Core
{
    public readonly struct StringPair : IEquatable<StringPair>
    {
        public StringPair(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; }

        public string Value { get; }

        public static bool operator ==(StringPair left, StringPair right) => left.Equals(right);

        public static bool operator !=(StringPair left, StringPair right) => !left.Equals(right);

        public bool Equals(StringPair other) =>
            string.Equals(Key, other.Key, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }

            return obj is StringPair stringPair && Equals(stringPair);
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

            if (Key is {})
            {
                toStringBuilder.Append(Key);
            }

            toStringBuilder.Append(", \"");

            if (Value is {})
            {
                toStringBuilder.Append(Value);
            }

            toStringBuilder.Append("\"]");
            return toStringBuilder.ToString();
        }
    }
}