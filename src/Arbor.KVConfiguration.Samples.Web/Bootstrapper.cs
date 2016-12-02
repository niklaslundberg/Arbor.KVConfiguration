using System.Reflection;
using System.Web.Mvc;

using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Schema;
using Arbor.KVConfiguration.SystemConfiguration;

using Autofac;
using Autofac.Integration.Mvc;

namespace Arbor.KVConfiguration.Samples.Web
{
    public static class Bootstrapper
    {
        public static void Start()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            RegisterConfiguration(builder);
            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            IKeyValueConfiguration keyValueConfiguration = container.Resolve<IKeyValueConfiguration>();

            KVConfigurationManager.Initialize(keyValueConfiguration);
        }

        private static void RegisterConfiguration(ContainerBuilder builder)
        {
            IKeyValueConfiguration sourceConfiguration = new SourceKeyValueConfiguration(typeof(Bootstrapper).Assembly);
            IKeyValueConfiguration appSettingsKeyValueConfiguration = new AppSettingsKeyValueConfiguration();
            IKeyValueConfiguration userConfiguration = new UserConfiguration.UserConfiguration(new ConfigurationWithFallback(appSettingsKeyValueConfiguration, sourceConfiguration));
            IKeyValueConfiguration expanded = new ExpandConfiguration(userConfiguration);

            builder.RegisterInstance(expanded)
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
