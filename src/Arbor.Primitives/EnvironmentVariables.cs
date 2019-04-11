using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Arbor.Primitives
{
    public class EnvironmentVariables
    {
        public EnvironmentVariables(IReadOnlyDictionary<string, string> variables)
        {
            Variables = variables ?? throw new ArgumentNullException(nameof(variables));
        }

        public IReadOnlyDictionary<string, string> Variables { get; }

        private static ImmutableDictionary<string, string> GetAll()
        {
            IDictionary environmentVariables = Environment.GetEnvironmentVariables();

            ImmutableDictionary<string, string> all = environmentVariables
                .OfType<DictionaryEntry>()
                .ToImmutableDictionary(entry => (string) entry.Key,
                    entry => (string) entry.Value,
                    StringComparer.OrdinalIgnoreCase);

            return all;
        }

        public static EnvironmentVariables GetEnvironmentVariables()
        {
            return new EnvironmentVariables(GetAll());
        }
    }
}
