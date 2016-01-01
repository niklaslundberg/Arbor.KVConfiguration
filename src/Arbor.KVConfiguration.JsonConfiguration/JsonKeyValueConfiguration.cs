using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Arbor.KVConfiguration.Core;

namespace Arbor.KVConfiguration.JsonConfiguration
{
    public class JsonKeyValueConfiguration : IKeyValueConfiguration
    {
        private readonly string _fileFullPath;

        public JsonKeyValueConfiguration(string fileFullPath)
        {
            _fileFullPath = fileFullPath;
        }

        public string this[string key] => ValueOrDefault(key);

        public string ValueOrDefault(string key)
        {
            return ValueOrDefault(key, "");
        }

        public string ValueOrDefault(string key, string defaultValue)
        {
            throw new NotImplementedException();
        }
    }
}
