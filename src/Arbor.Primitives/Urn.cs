using System;
using System.Linq;

namespace Arbor.Primitives
{
    public sealed class Urn : IEquatable<Urn>
    {
        private const string DoubleSeparator = "::";
        public const char Separator = ':';
        private static readonly char[] InvalidCharacters = {'/', '\\'};
        private static readonly string[] _componentChars = {"?=", "?+", "#"};

        public Urn(string originalValue)
        {
            if (string.IsNullOrWhiteSpace(originalValue))
            {
                throw new ArgumentException(PrimitivesResources.ArgumentIsNullOrWhitespace, nameof(originalValue));
            }

            string trimmed = originalValue.Trim();

            if (!IsUri(trimmed, out var uri))
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
            string fullName = trimmed.Substring("urn".Length + "::".Length + Nid.Length);

            ReadOnlySpan<char> schemeSlice = chars.Slice(0, 3);
#pragma warning disable CA1308 // Normalize strings to uppercase
            Scheme = schemeSlice.ToString().ToLowerInvariant();
#pragma warning restore CA1308 // Normalize strings to uppercase

            OriginalValue = trimmed;

            bool parsed = TryParseComponents(fullName, out string? nss, out string? r, out string? q, out string? f);

            if (!parsed)
            {
                throw new FormatException("Could not get components");
            }

            Nss = nss!;

            RComponent = r!;
            QComponent = q!;
            Fragment = f!;

            Normalized = "urn:" + Nid.ToLowerInvariant() + ":" + Nss;
            FullName = trimmed;
        }

        public string FullName { get; }

        private static bool TryParseComponents(string fullName,
            out string? nss,
            out string? rComponent,
            out string? qComponent,
            out string? fragment)
        {
            int fragmentIndex = fullName.IndexOf('#');
            int qComponentIndex = fullName.IndexOf("?=", StringComparison.Ordinal);
            int rComponentIndex = fullName.IndexOf("?+", StringComparison.Ordinal);

            if (qComponentIndex >= 0 && rComponentIndex >= 0 && qComponentIndex < rComponentIndex)
            {
                fragment = null;
                qComponent = null;
                rComponent = null;
                nss = null;
                return false;
            }

            if (fragmentIndex >= 0 && qComponentIndex >= 0 && fragmentIndex < qComponentIndex)
            {
                fragment = null;
                qComponent = null;
                rComponent = null;
                nss = null;
                return false;
            }

            if (fragmentIndex >= 0 && rComponentIndex >= 0 && fragmentIndex < rComponentIndex)
            {
                fragment = null;
                qComponent = null;
                rComponent = null;
                nss = null;
                return false;
            }

            if (fragmentIndex >= 0 && fullName.Length - fragmentIndex >= 0)
            {
                fragment = fullName.Substring(fragmentIndex + 1);
            }
            else
            {
                fragment = "";
            }

            if (fragment.Contains("?=") || fragment.Contains("?+") || fragment.Contains("#"))
            {
                fragment = null;
                qComponent = null;
                rComponent = null;
                nss = null;
                return false;
            }

            int qComponentLength = fragmentIndex >= 0
                ? fullName.Length - fragmentIndex - 1
                : fullName.Length - qComponentIndex - 2;

            if (qComponentLength > 0)
            {
                qComponent = qComponentIndex >= 0 ? fullName.Substring(qComponentIndex + 2, qComponentLength) : "";
            }
            else
            {
                qComponent = "";
            }

            int rComponentLength = 0;

            if (rComponentIndex >= 0)
            {
                if (qComponentIndex >= 0)
                {
                    rComponentLength = fullName.Length - qComponentIndex - 2;
                }
                else if (fragmentIndex >= 0)
                {
                    rComponentLength = fullName.Length - fragmentIndex - 2;
                }
                else
                {
                    rComponentLength = fullName.Length - rComponentIndex - 2;
                }
            }

            if (rComponentLength > 0)
            {
                if (qComponentIndex >= 0 && qComponent.Length > 0)
                {
                    rComponent = fullName.Substring(rComponentIndex + 2, qComponentIndex - rComponentIndex - 2);
                }
                else if (qComponentIndex >= 0)
                {
                    rComponent = fullName.Substring(rComponentIndex + 2, rComponentLength - 1);
                }
                else if (fragmentIndex >= 0)
                {
                    rComponent = fullName.Substring(rComponentIndex + 2, rComponentLength + 1);
                }
                else
                {
                    rComponent = "";
                }
            }
            else
            {
                rComponent = "";
            }

            if (rComponentIndex >= 0)
            {
                nss = fullName.Substring(0, rComponentIndex);
            }
            else if (qComponentIndex >= 0)
            {
                nss = fullName.Substring(0, qComponentIndex);
            }
            else if (fragmentIndex >= 0)
            {
                nss = fullName.Substring(0, fragmentIndex);
            }
            else
            {
                nss = fullName;
            }

            return true;
        }

        public string Normalized { get; }

        public string Scheme { get; }

        public string Nid { get; }

        public string? Name
        {
            get
            {
                string? lastOrDefault = OriginalValue.Split(
                    new[] {Separator},
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

                int separators = OriginalValue.Count(character => character.Equals(Separator));

                if (separators <= 1)
                {
                    throw new InvalidOperationException($"The urn '{OriginalValue}' has no parent");
                }

                string parent = OriginalValue.Substring(0, lastSeparatorIndex);

                return new Urn(parent);
            }
        }

        public string Nss { get; }
        public string QComponent { get; }
        public string RComponent { get; }
        public string Fragment { get; }

        public bool Equals(Urn? other)
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

            var caseSensitiveParts = CaseInsensitiveParts(Nss);
            var otherParts = CaseInsensitiveParts(other.Nss);

            return caseSensitiveParts.SequenceEqual(otherParts);
        }

        public static bool operator ==(Urn left, Urn right) => Equals(left, right);

        public static bool operator !=(Urn left, Urn right) => !Equals(left, right);

        public static bool TryParse(string? originalValue, out Urn? result)
        {
            if (originalValue is null || string.IsNullOrWhiteSpace(originalValue))
            {
                result = null;
                return false;
            }

            string trimmed = originalValue.Trim();

            if (!IsUri(trimmed, out var uri))
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

            bool parsedComponents = TryParseComponents(trimmed, out string? nss, out string? r, out string? q, out string? f);

            if (!parsedComponents)
            {
                result = null;
                return false;
            }

            result = new Urn(trimmed);

            return true;
        }

        public override string ToString() => FullName;

        public bool IsInHierarchy(Urn? other)
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

        private static ReadOnlySpan<char> CaseInsensitiveParts(string nss)
        {
            int firstIndex = -1;

            foreach (var componentChar in _componentChars)
            {
                int index = nss.IndexOf(componentChar, StringComparison.Ordinal);

                if (index >= 0)
                {
                    if (firstIndex >= 0 && index <= firstIndex)
                    {
                        firstIndex = index;
                    }
                    else
                    {
                        firstIndex = index;
                    }
                }
            }

            if (firstIndex >= 0)
            {
                return nss.AsSpan().Slice(0, firstIndex);
            }

            return nss.AsSpan();
        }

        public override bool Equals(object? obj) => Equals(obj as Urn);

        public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(OriginalValue);

        private static bool IsWellFormedUriString(string originalValue) =>
            Uri.IsWellFormedUriString(originalValue, UriKind.Absolute);

        private static bool HasUrnScheme(Uri uri) => uri.Scheme.Equals("urn", StringComparison.OrdinalIgnoreCase);

        private static bool IsUri(string originalValue, out Uri uri) =>
            Uri.TryCreate(originalValue, UriKind.Absolute, out uri);

        public static Urn Parse(string attemptedValue)
        {
            if (!TryParse(attemptedValue, out var urn))
            {
                throw new FormatException($"The attempted value '{attemptedValue}' is not a valid URN");
            }

            return urn!;
        }
    }
}