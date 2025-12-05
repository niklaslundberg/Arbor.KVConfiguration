using System;
using System.Collections;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;

namespace Arbor.Primitives;

public abstract class EnvironmentVariables
{
    public abstract IReadOnlyDictionary<string, string> Variables { get; }


    private static readonly Lazy<SystemEnvironmentVariables> Lazy = new(() => new SystemEnvironmentVariables());

    public static EnvironmentVariables System => Lazy.Value;

    public sealed class SystemEnvironmentVariables: EnvironmentVariables
    {
        internal SystemEnvironmentVariables(StringComparer? stringComparer = null) => Variables = GetAll(stringComparer ?? DefaultStringComparer);

        public override IReadOnlyDictionary<string, string> Variables { get; }

        private static FrozenDictionary<string, string> GetAll(StringComparer stringComparer)
        {
            var environmentVariables = Environment.GetEnvironmentVariables();

            return environmentVariables
                .OfType<DictionaryEntry>()
                .ToFrozenDictionary(entry => (string)entry.Key,
                    entry => (string?)entry.Value ?? "",
                    stringComparer);
        }

        public static readonly StringComparer DefaultStringComparer =
            Environment.OSVersion.Platform is PlatformID.Unix or PlatformID.MacOSX
                ? StringComparer.Ordinal
                : StringComparer.OrdinalIgnoreCase;

        public static EnvironmentVariables GetEnvironmentVariables(StringComparer stringComparer)
        {
            stringComparer.ThrowIfNull();

            return new SystemEnvironmentVariables(stringComparer);
        }
    }
}