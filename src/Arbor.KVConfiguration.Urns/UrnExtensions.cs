using System;
using System.Linq;
using Arbor.Primitives;

namespace Arbor.KVConfiguration.Urns
{
    public static class UrnExtensions
    {
        public static int NamespaceParts(this Urn urn)
        {
            if (urn is null)
            {
                throw new ArgumentNullException(nameof(urn));
            }

            return urn.OriginalValue.Count(c => c == Urn.Separator) + 1;
        }

        public static bool IsUrn(this string value) => Urn.TryParse(value, out _);
    }
}