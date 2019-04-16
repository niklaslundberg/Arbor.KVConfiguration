using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Arbor.KVConfiguration.Core.Extensions.CommandLine
{
    public static class CommandLineAppExtensions
    {
        private const char SplitChar = '=';
        private static readonly char[] VariableAssignmentCharacter = { SplitChar };

        public static IKeyValueConfiguration ToKeyValueConfiguration(this IEnumerable<string> args)
        {
            var nameValueCollection = new NameValueCollection(StringComparer.OrdinalIgnoreCase);

            foreach (string arg in args.Where(a =>
                a.Count(c => c == SplitChar) == 1 && a.Length >= 3))
            {
                string[] parts = arg.Split(VariableAssignmentCharacter, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                {
                    continue;
                }

                string key = parts[0];
                string value = parts[1];

                nameValueCollection.Add(key, value);
            }

            var inMemoryKeyValueConfiguration = new InMemoryKeyValueConfiguration(nameValueCollection);

            return inMemoryKeyValueConfiguration;
        }

        public static AppSettingsBuilder AddCommandLineArgsSettings(
            this AppSettingsBuilder builder,
            IEnumerable<string> args)
        {
            if (args is null)
            {
                return builder;
            }

            IKeyValueConfiguration inMemoryKeyValueConfiguration = args.ToKeyValueConfiguration();

            return builder.Add(inMemoryKeyValueConfiguration);
        }
    }
}
