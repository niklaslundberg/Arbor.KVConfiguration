using System.Collections.Generic;

using Arbor.KVConfiguration.Schema;

namespace Arbor.KVConfiguration.JsonConfiguration
{
    public class JsonFileReader
    {
        private string _fileFullPath;

        public JsonFileReader(string fileFullPath)
        {
            _fileFullPath = fileFullPath;
        }


        public IReadOnlyCollection<KeyValueConfigurationItem> ReadConfiguration()
        {
            return new List<KeyValueConfigurationItem>();
        }
    }
}