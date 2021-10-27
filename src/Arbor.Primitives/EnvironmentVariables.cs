using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Arbor.Primitives
{
    public class EnvironmentVariables
    {
        public EnvironmentVariables(IReadOnlyDictionary<string, string> variables) =>
            Variables = variables ?? throw new ArgumentNullException(nameof(variables));

        public IReadOnlyDictionary<string, string> Variables { get; }

        private static ImmutableDictionary<string, string> GetAll(StringComparer stringComparer)
        {
            var environmentVariables = Environment.GetEnvironmentVariables();

            return environmentVariables
                .OfType<DictionaryEntry>()
                     .ToImmutableDictionary(entry => (string)entry.Key,
                    entry => (string?)entry.Value ?? "",
                    stringComparer);
        }

        public static readonly StringComparer DefaultStringComparer =
            Environment.OSVersion.Platform is PlatformID.Unix or PlatformID.MacOSX
                ? StringComparer.Ordinal
                : StringComparer.OrdinalIgnoreCase;

        public static EnvironmentVariables GetEnvironmentVariables() => new(GetAll(DefaultStringComparer));
        public static EnvironmentVariables GetEnvironmentVariables(StringComparer stringComparer)
        {
            if (stringComparer is null)
            {
                throw new ArgumentNullException(nameof(stringComparer));
            }

            return new EnvironmentVariables(GetAll(stringComparer));
        }
    }
}