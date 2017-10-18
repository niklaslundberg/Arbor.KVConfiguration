using System.Reflection;
using System.Web.Mvc;
using Arbor.KVConfiguration.Core;
using Autofac;
using Autofac.Integration.Mvc;
using Arbor.KVConfiguration.UserConfiguration;

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
        }

        private static void RegisterConfiguration(ContainerBuilder builder)
        {
            ConfigurationInitializer.EnsureConfigurationIsInitialized();

            builder.RegisterInstance(StaticKeyValueConfigurationManager.AppSettings)
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();
        }
    }
}
