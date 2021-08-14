using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Arbor.Primitives
{
    /// <summary>
    /// Implements https://tools.ietf.org/html/rfc8141
    /// </summary>
    public readonly struct Urn : IEquatable<Urn>
    {
        private const string DoubleSeparator = "::";
        public const char Separator = ':';
        private static readonly string[] ComponentChars = {"?=", "?+", "#"};
        private readonly ReadOnlyMemory<char> _nid;
        private readonly ReadOnlyMemory<char> _nss;
        private readonly ReadOnlyMemory<char> _rComponent;
        private readonly ReadOnlyMemory<char> _qComponent;
        private readonly ReadOnlyMemory<char> _fComponent;

        private Urn(string originalValue, ReadOnlyMemory<char> nid, ReadOnlyMemory<char> nss, ReadOnlyMemory<char> r, ReadOnlyMemory<char> q, ReadOnlyMemory<char> f)
        {
            _nid = nid;

            OriginalValue = originalValue;

            _nss = nss;

            _rComponent = r;
            _qComponent = q;
            _fComponent = f;
        }

        public string NameString => _nid.Length == 0 ? "N/A" : $"urn{OriginalValue[3..]}";

        private static bool TryParseComponents(ReadOnlyMemory<char> fullName,
            out ReadOnlyMemory<char> nss,
            out ReadOnlyMemory<char> rComponent,
            out ReadOnlyMemory<char> qComponent,
            out ReadOnlyMemory<char> fragment)
        {
            int fragmentIndex = fullName.Span.IndexOf('#');
            int qComponentIndex = fullName.ToString().IndexOf("?=", StringComparison.Ordinal);
            int rComponentIndex = fullName.ToString().IndexOf("?+", StringComparison.Ordinal);

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
                fragment = fullName[(fragmentIndex + 1)..];
            }
            else
            {
                fragment = ReadOnlyMemory<char>.Empty;
            }

            if (fragment.ToString().Contains("?=") || fragment.ToString().Contains("?+") || fragment.ToString().Contains("#"))
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
                qComponent = qComponentIndex >= 0 ? fullName.Slice(qComponentIndex + 2, qComponentLength) : ReadOnlyMemory<char>.Empty;
            }
            else
            {
                qComponent = ReadOnlyMemory<char>.Empty;
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
                    rComponent = fullName.Slice(rComponentIndex + 2, qComponentIndex - rComponentIndex - 2);
                }
                else if (qComponentIndex >= 0)
                {
                    rComponent = fullName.Slice(rComponentIndex + 2, rComponentLength - 1);
                }
                else if (fragmentIndex >= 0)
                {
                    rComponent = fullName.Slice(rComponentIndex + 2, rComponentLength + 1);
                }
                else
                {
                    rComponent = ReadOnlyMemory<char>.Empty;
                }
            }
            else
            {
                rComponent = ReadOnlyMemory<char>.Empty;
            }

            if (rComponentIndex >= 0)
            {
                nss = fullName.Slice(0, rComponentIndex);
            }
            else if (qComponentIndex >= 0)
            {
                nss = fullName.Slice(0, qComponentIndex);
            }
            else if (fragmentIndex >= 0)
            {
                nss = fullName.Slice(0, fragmentIndex);
            }
            else
            {
                nss = fullName;
            }

            foreach (char c in nss.Span)
            {
                if (!c.IsAscii())
                {
                    fragment = null;
                    qComponent = null;
                    rComponent = null;
                    nss = null;
                    return false;
                }
            }

            return true;
        }

        public string AssignedName => $"urn:{Nid}:{Nss}";

        public string Scheme => "urn";

        public string Nid => _nid.ToString();

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

                return Parse(parent);
            }
        }

        public string Nss => _nss.ToString();
        public string QComponent => _qComponent.ToString();
        public string RComponent => _rComponent.ToString();
        public string FComponent => _fComponent.ToString();

        public static bool Equals(Urn @this, Urn other)
        {
            if (!@this.Scheme.Equals(other.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!@this.Nid.Equals(other.Nid, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var caseSensitiveParts = CaseInsensitiveParts(@this.Nss);
            var otherParts = CaseInsensitiveParts(other.Nss);

            return caseSensitiveParts.SequenceEqual(otherParts);
        }

        public static bool operator ==(Urn left, Urn right) => Equals(left, right);

        public static bool operator !=(Urn left, Urn right) => !Equals(left, right);

        public static bool TryParse(string? originalValue, out Urn? result)
        {
            if (originalValue is null || string.IsNullOrWhiteSpace(originalValue))
            {
                result = default;
                return false;
            }

            string trimmed = originalValue.Trim();

            if (!IsUri(trimmed, out var uri))
            {
                result = default;
                return false;
            }

            if (!HasUrnScheme(uri))
            {
                result = default;
                return false;
            }

            if (!IsWellFormedUriString(trimmed))
            {
                result = default;
                return false;
            }

            if (trimmed.Contains(DoubleSeparator, StringComparison.OrdinalIgnoreCase))
            {
                throw new FormatException("Urn contains invalid double colon");
            }

            var chars = trimmed.AsMemory();

            if (chars.Span.IndexOf(Separator) < 0)
            {
                result = default;
                return false;
            }

            ReadOnlyMemory<char> nidSub = chars[(chars.Span.IndexOf(Separator) + 1)..];

            if (nidSub.Span.IndexOf(Separator) < 0)
            {
                result = default;
                return false;
            }

            if (nidSub.Span.IndexOf(Separator) < 0)
            {
                throw new FormatException($"Attempted value '{originalValue}' is not a valid urn");
            }

            ReadOnlyMemory<char> nidSlice = nidSub.Slice(0, nidSub.Span.IndexOf(Separator));

            foreach (char c in nidSlice.Span)
            {
                if (!(char.IsLetterOrDigit(c) || c == '-'))
                {
                    throw new FormatException("Only alphanumeric characters allowed");
                }
            }

            if (nidSlice.Span.EndsWith("-".AsSpan()))
            {
                throw new FormatException("Nid cannot end with '-'");
            }

            bool parsedComponents = TryParseComponents(nidSub[(nidSlice.Length + 1)..], out ReadOnlyMemory<char> nss, out ReadOnlyMemory<char> r, out ReadOnlyMemory<char> q, out ReadOnlyMemory<char> f);

            if (!parsedComponents)
            {
                result = default;
                return false;
            }

            char[] nidArray = new char[nidSlice.Length];
            var nid = new Span<char>(nidArray );
            nidSlice.Span.ToLowerInvariant(nid);
            result = new Urn(trimmed, new ReadOnlyMemory<char>(nidArray), nss!, r, q,f);

            return true;
        }

        public override string ToString() => _nid.Length == 0? "N/A" : NameString;

        public bool IsInHierarchy(Urn other)
        {
            if (other.Nid.Length == 0)
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

            foreach (string componentChar in ComponentChars)
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
                return nss.AsSpan()[..firstIndex];
            }

            return nss.AsSpan();
        }

        public bool Equals(Urn other) => Equals(this, other);
        public override bool Equals(object? obj) => obj is Urn urn && Equals(urn);

        public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(OriginalValue);

        private static bool IsWellFormedUriString(string originalValue) =>
            Uri.IsWellFormedUriString(originalValue, UriKind.Absolute);

        private static bool HasUrnScheme(Uri uri) => uri.Scheme.Equals("urn", StringComparison.OrdinalIgnoreCase);

        private static bool IsUri(string originalValue, [NotNullWhen(true)] out Uri? uri) =>
            Uri.TryCreate(originalValue, UriKind.Absolute, out uri);

        public static Urn Parse(string attemptedValue)
        {
            if (string.IsNullOrWhiteSpace(attemptedValue))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(attemptedValue));
            }

            if (!TryParse(attemptedValue, out var urn))
            {
                throw new FormatException($"The attempted value '{attemptedValue}' is not a valid URN");
            }

            return urn!.Value;
        }
    }
}