using System;
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
                throw new ArgumentException("Argument is null or whitespace", nameof(originalValue));
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
                throw new FormatException("Urn contains invalid characters");
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

            OriginalValue = trimmed;
        }

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

        public static bool operator ==(Urn left, Urn right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Urn left, Urn right)
        {
            return !Equals(left, right);
        }

        public static bool TryParse(string originalValue, out Urn result)
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

            result = new Urn(trimmed);

            return true;
        }

        public override string ToString()
        {
            return OriginalValue;
        }

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

            return string.Equals(OriginalValue, other.OriginalValue, StringComparison.OrdinalIgnoreCase);
        }

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

            return Equals((Urn) obj);
        }

        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(OriginalValue);
        }

        private static bool IsWellFormedUriString(string originalValue)
        {
            return Uri.IsWellFormedUriString(originalValue, UriKind.Absolute);
        }

        private static bool HasUrnScheme(Uri uri)
        {
            return uri.Scheme.Equals("urn", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsUri(string originalValue, out Uri uri)
        {
            return Uri.TryCreate(originalValue, UriKind.Absolute, out uri);
        }
    }
}
