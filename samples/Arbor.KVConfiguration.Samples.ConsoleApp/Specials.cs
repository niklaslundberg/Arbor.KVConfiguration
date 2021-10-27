using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Arbor.KVConfiguration.Samples.ConsoleApp
{
    public static class Specials
    {
        public static readonly ImmutableDictionary<string, string?> Special = new Dictionary<string, string?>
        {
            {"[EMPTY]", string.Empty}, {"[NULL]", null}, {"[BLANK]", "\t"}
        }.ToImmutableDictionary();

        public static string GetDisplayValue(string value)
        {
            var keyValuePairs = Special
                .Where(pair => Equals(value, pair.Value))
                .ToArray();

            if (!keyValuePairs.Any())
            {
                return value;
            }

            var first = keyValuePairs.First();

            return first.Key;
        }
    }
}