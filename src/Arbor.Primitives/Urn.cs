using System;
using System.Collections.Generic;
using System.Linq;

namespace Arbor.Primitives
{
    public class Urn : IEquatable<Urn>
    {
        private static readonly char[] InvalidCharacters = {'/', '\\'};
        private const string DoubleSeparator = "::";
        public const char Separator = ':';

        public Urn(string originalValue)
        {
            if (string.IsNullOrWhiteSpace(originalValue))
            {
                throw new ArgumentException(PrimitivesResources.ArgumentIsNullOrWhitespace, nameof(originalValue));
            }

            string trimmed = originalValue.Trim();

            if (!IsUri(trimmed, out Uri uri))
            {
                throw new FormatException($"Invalid urn '{trimmed}'");
            }

            if (!HasUrnScheme(uri))
            {
                throw new FormatException($"Invalid urn '{trimmed}'");
            }

            if (trimmed.IndexOfAny(InvalidCharacters) >= 0)
            {
                throw new FormatException(PrimitivesResources.UrnContainsInvalidCharacters);
            }

            if (trimmed.IndexOf(DoubleSeparator, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                throw new FormatException("Urn contains invalid double colon");
            }

            if (!IsWellFormedUriString(trimmed))
            {
                throw new FormatException($"Invalid urn '{trimmed}'");
            }

            var chars = trimmed.AsSpan();

            if (chars.IndexOf(Separator) < 0)
            {
                throw new InvalidOperationException();
            }

            ReadOnlySpan<char> nidSub = chars.Slice(chars.IndexOf(Separator) + 1);

            if (nidSub.IndexOf(Separator) < 0)
            {
                throw new InvalidOperationException($"Attempted value '{originalValue}' is not a valid urn");
            }

            ReadOnlySpan<char> nidSlice = nidSub.Slice(0, nidSub.IndexOf(Separator));

            foreach (char c in nidSlice)
            {
                if (!c.IsAscii())
                {
                    throw new InvalidOperationException("Only ascii characters allowed");
                }
            }

            Nid = nidSlice.ToString();

            ReadOnlySpan<char> schemeSlice = chars.Slice(0, 3);
#pragma warning disable CA1308 // Normalize strings to uppercase
            Scheme = schemeSlice.ToString().ToLowerInvariant();
#pragma warning restore CA1308 // Normalize strings to uppercase

            OriginalValue = trimmed;
        }

        public string Scheme { get; }

        public string Nid { get; }

        public string Name
        {
            get
            {
                string lastOrDefault = OriginalValue.Split(
                    new[]
                    {
                        Separator
                    },
                    StringSplitOptions.RemoveEmptyEntries).LastOrDefault();

                if (string.IsNullOrWhiteSpace(lastOrDefault))
                {
                    throw new InvalidOperationException($"Could not get subNamespace from urn \'{OriginalValue}\'");
                }

                return lastOrDefault;
            }
        }

        public string OriginalValue { get; }

        public Urn Parent
        {
            get
            {
                int lastSeparatorIndex = OriginalValue.LastIndexOf(Separator);

                if (lastSeparatorIndex < 0)
                {
                    throw new InvalidOperationException($"Could not get parent from urn '{OriginalValue}'");
                }

                int separators = OriginalValue.Count(c => c.Equals(Separator));

                if (separators <= 1)
                {
                    throw new InvalidOperationException($"The urn '{OriginalValue}' has no parent");
                }

                string parent = OriginalValue.Substring(0, lastSeparatorIndex);

                return new Urn(parent);
            }
        }

        public static bool operator ==(Urn left, Urn right) => Equals(left, right);

        public static bool operator !=(Urn left, Urn right) => !Equals(left, right);

        public static bool TryParse(string originalValue, out Urn? result)
        {
            if (string.IsNullOrWhiteSpace(originalValue))
            {
                result = null;
                return false;
            }

            string trimmed = originalValue.Trim();

            if (!IsUri(trimmed, out Uri uri))
            {
                result = null;
                return false;
            }

            if (!HasUrnScheme(uri))
            {
                result = null;
                return false;
            }

            if (!IsWellFormedUriString(trimmed))
            {
                result = null;
                return false;
            }

            var chars = trimmed.AsSpan();

            if (chars.IndexOf(Separator) < 0)
            {
                result = null;
                return false;
            }

            ReadOnlySpan<char> nidSub = chars.Slice(chars.IndexOf(Separator) + 1);

            if (nidSub.IndexOf(Separator) < 0)
            {
                result = null;
                return false;
            }

            result = new Urn(trimmed);

            return true;
        }

        public override string ToString() => OriginalValue;

        public bool IsInHierarchy(Urn other)
        {
            if (other is null)
            {
                return false;
            }

            string[] parts = OriginalValue.Split(Separator);
            string[] otherParts = other.OriginalValue.Split(Separator);

            if (parts.Length < otherParts.Length)
            {
                return false;
            }

            for (int i = 0; i < otherParts.Length; i++)
            {
                string otherPart = otherParts[i];
                string partValue = parts[i];

                if (!otherPart.Equals(partValue, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }

        public bool Equals(Urn other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (!Scheme.Equals(other.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!Nid.Equals(other.Nid, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var caseSensitiveParts = CaseInsensitiveParts(this);
            var otherParts = CaseInsensitiveParts(other);

            if (!caseSensitiveParts.SequenceEqual(otherParts))
            {
                return false;
            }

            return string.Equals(OriginalValue, other.OriginalValue, StringComparison.OrdinalIgnoreCase);
        }

        private static ReadOnlySpan<char> CaseInsensitiveParts(Urn urn) => urn.OriginalValue.AsSpan().Slice(urn.Scheme.Length + urn.Nid.Length + 1);

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((Urn)obj);
        }

        public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(OriginalValue);

        private static bool IsWellFormedUriString(string originalValue) => Uri.IsWellFormedUriString(originalValue, UriKind.Absolute);

        private static bool HasUrnScheme(Uri uri) => uri.Scheme.Equals("urn", StringComparison.OrdinalIgnoreCase);

        private static bool IsUri(string originalValue, out Uri uri) => Uri.TryCreate(originalValue, UriKind.Absolute, out uri);

        public static Urn Parse(string attemptedValue)
        {
            if (!TryParse(attemptedValue, out Urn? urn))
            {
                throw new FormatException($"The attempted value '{attemptedValue}' is not a valid URN");
            }

            return urn!;
        }
    }
}
