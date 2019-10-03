using System;
using System.Collections.Generic;
using System.Text;
using Arbor.KVConfiguration.Core;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Tests.Unit
{
    internal static class MultipleExtensions
    {
        public static string Print([NotNull] this IEnumerable<MultipleValuesStringPair> multipleValuesStringPairs)
        {
            if (multipleValuesStringPairs is null)
            {
                throw new ArgumentNullException(nameof(multipleValuesStringPairs));
            }

            var builder = new StringBuilder();

            foreach (MultipleValuesStringPair multipleValuesStringPair in multipleValuesStringPairs)
            {
                builder.AppendLine(multipleValuesStringPair.Key);
                foreach (string value in multipleValuesStringPair.Values)
                {
                    builder.AppendLine($" {value}");
                }
            }

            return builder.ToString();
        }
    }
}
