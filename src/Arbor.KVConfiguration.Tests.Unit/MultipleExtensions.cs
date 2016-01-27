using System.Collections.Generic;
using System.Text;

using Arbor.KVConfiguration.Core;

namespace Arbor.KVConfiguration.Tests.Unit
{
    public static class MultipleExtensions
    {
        public static string Print(this IEnumerable<MultipleValuesStringPair> multipleValuesStringPairs)
        {
            StringBuilder builder = new StringBuilder();

            foreach (MultipleValuesStringPair multipleValuesStringPair in multipleValuesStringPairs)
            {
                builder.AppendLine(multipleValuesStringPair.Key);
                foreach (string value in multipleValuesStringPair.Values)
                {
                    builder.AppendLine(" " + value);
                }
            }

            return builder.ToString();
        }
    }
}