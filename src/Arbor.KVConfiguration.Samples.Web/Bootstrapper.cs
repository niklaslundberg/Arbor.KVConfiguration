using System.Reflection;
using System.Web.Mvc;

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
        }

        private static void RegisterConfiguration(ContainerBuilder builder)
        {
            builder.Register(context => new UserConfiguration.UserConfiguration(new AppSettingsKeyValueConfiguration()))
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}