using System;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core
{
    public readonly struct MultipleValuesStringPair : IEquatable<MultipleValuesStringPair>
    {
        private readonly ImmutableArray<string> _values;

        public MultipleValuesStringPair([NotNull] string key, ImmutableArray<string> values)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            _values = values;
            HasNonEmptyValue = values.Any(value => !string.IsNullOrWhiteSpace(value));
        }

        public string Key { get; }

        public ImmutableArray<string> Values
        {
            get
            {
                if (_values.IsDefault)
                {
                    return ImmutableArray<string>.Empty;
                }

                return _values;
            }
        }

        public bool HasNonEmptyValue { get; }

        public bool HasSingleValue => Values.Length == 1;

        public static bool operator ==(MultipleValuesStringPair left, MultipleValuesStringPair right) =>
            left.Equals(right);

        public static bool operator !=(MultipleValuesStringPair left, MultipleValuesStringPair right) =>
            !left.Equals(right);

        public bool Equals(MultipleValuesStringPair other) =>
            string.Equals(Key, other.Key, StringComparison.OrdinalIgnoreCase) &&
            Values.SequenceEqual(other.Values, StringComparer.OrdinalIgnoreCase);

        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }

            return obj is MultipleValuesStringPair multipleValuesStringPair && Equals(multipleValuesStringPair);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Key?.GetHashCode() ?? 0) * 397) ^ Values.GetHashCode();
            }
        }
    }
}