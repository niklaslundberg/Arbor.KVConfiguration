using Arbor.KVConfiguration.Urns;
using Microsoft.Extensions.Configuration;

namespace Arbor.KVConfiguration.Microsoft.Extensions.Configuration.Urns
{
    public class KVConfigurationBinder
    {
        public static TOptions Bind<TOptions>(IConfiguration config) where TOptions: class
        {
            KVAdapter adapter=  new KVAdapter(config);

            return UrnKeyValueExtensions.GetInstance<TOptions>(adapter);
        }
    }
}