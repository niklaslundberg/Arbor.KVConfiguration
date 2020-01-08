using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Arbor.KVConfiguration.GlobalTool
{
    public class ArgParser
    {
        public ImmutableArray<KeyValuePair<string, string>> Parse(IEnumerable<string> parameters) => parameters
            .Where(parameter => !string.IsNullOrWhiteSpace(parameter)
                                && parameter.Contains("="))
            .Select(parameter =>
            {
                string[] parts = parameter.Split("=");

                if (parts.Length != 2)
                {
                    return default;
                }

                if (string.IsNullOrWhiteSpace(parts[0]))
                {
                    return default;
                }

                if (string.IsNullOrWhiteSpace(parts[1]))
                {
                    return default;
                }

                return new KeyValuePair<string, string>(parts[0], parts[1]);
            })
            .Where(pair => pair.Key is { })
            .ToImmutableArray();
    }
}