using System.Diagnostics;
using System.Web.Hosting;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Core.Decorators;
using Arbor.KVConfiguration.Core.Metadata;
using Arbor.KVConfiguration.JsonConfiguration;
using Arbor.KVConfiguration.SystemConfiguration;
using Arbor.KVConfiguration.SystemConfiguration.Providers;

namespace Arbor.KVConfiguration.Samples.Web
{   
    public static class ConfigurationInitializer
    {
        private static readonly object _MutexLock = new object();

        public static void EnsureConfigurationIsInitialized()
        {
            if (StaticKeyValueConfigurationManager.IsInitialized)
            {
                return;
            }

            lock (_MutexLock)
            {
                if (StaticKeyValueConfigurationManager.IsInitialized)
                {
                    return;
                }

                Initialize();
            }
        }

        static void Initialize()
        {
            MultiSourceKeyValueConfiguration keyValueConfiguration = KeyValueConfigurationManager
                .Add(new ReflectionKeyValueConfiguration(typeof(Bootstrapper).Assembly))
                .Add(new AppSettingsKeyValueConfiguration())
                .Add(new UserConfiguration.UserConfiguration())
                .Add(new JsonKeyValueConfiguration(HostingEnvironment.MapPath("~/appsettings.json")))
                .DecorateWith(new ExpandKeyValueConfigurationDecorator())
                .DecorateWith(new AddSuffixDecorator("!"))
                .Build(message => Debug.WriteLine(message));

            StaticKeyValueConfigurationManager.Initialize(keyValueConfiguration);
        }
    }
}
