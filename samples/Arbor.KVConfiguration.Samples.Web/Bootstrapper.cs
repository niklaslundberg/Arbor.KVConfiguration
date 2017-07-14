using System.Diagnostics;
using System.Reflection;
using System.Web.Hosting;
using System.Web.Mvc;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Core.Decorators;
using Arbor.KVConfiguration.JsonConfiguration;
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
            MultiSourceKeyValueConfiguration keyValueConfiguration = KeyValueConfigurationManager
                .Add(new ReflectionKeyValueConfiguration(typeof(Bootstrapper).Assembly))
                .Add(new AppSettingsKeyValueConfiguration())
                .Add(new UserConfiguration.UserConfiguration())
                .Add(new JsonKeyValueConfiguration(HostingEnvironment.MapPath("~/appsettings.json")))
                .DecorateWith(new ExpandKeyValueConfigurationDecorator())
                .DecorateWith(new AddSuffixDecorator("!"))
                .Build(message => Debug.WriteLine(message));

            builder.RegisterInstance(keyValueConfiguration)
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();
        }
    }
}
