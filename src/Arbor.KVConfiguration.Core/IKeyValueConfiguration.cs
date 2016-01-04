using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbor.KVConfiguration.Core
{
    public interface IKeyValueConfiguration
    {
        string this[string key] { get; }

        string ValueOrDefault(string key);
        string ValueOrDefault(string key, string defaultValue);

        IReadOnlyCollection<string> AllKeys { get; }
        IReadOnlyCollection<KeyValuePair<string, string>> AllValues { get; }
        IReadOnlyCollection<KeyValuePair<string, IReadOnlyCollection<string>>> AllWithMultipleValues { get; }
    }
}
